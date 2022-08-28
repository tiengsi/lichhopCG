using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Models;
using WebApi.Models.Enums;

namespace WebApi.Services
{
  public interface IJobScheduleService
  {
    Task<int> CreateManualJob(int scheduleId, EJobScheduleType jobType, EScheduleStatus status);
    Task<int> CreateAutoJob(int scheduleId, EJobScheduleType jobType, EScheduleStatus status, DateTime scheduleTime);

    Task<List<JobScheduleModel>> GetManualJobIsNotExecute();
    Task<List<JobScheduleModel>> GetAutoJobIsNotExecute();

    Task UpdateJobIsExecuted(int obScheduleId);
  }

  public class JobScheduleService : IJobScheduleService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<JobScheduleService> _logger;
    private readonly IJobScheduleRepository _jobScheduleRepository;

    public JobScheduleService(
        IUnitOfWork unitOfWork,
        ILogger<JobScheduleService> logger,
        IJobScheduleRepository jobScheduleRepository)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
      _jobScheduleRepository = jobScheduleRepository;
    }

    public async Task<int> CreateManualJob(int scheduleId, EJobScheduleType jobType, EScheduleStatus status)
    {
      try
      {
        var model = new JobScheduleModel
        {
          CreatedDate = DateTime.Now,
          IsExecuted = false,
          JobScheduleType = jobType,
          ScheduleStatus = status,
          ScheduleId = scheduleId,
          IsSchedule=false
        };

        _jobScheduleRepository.Add(model);
        _unitOfWork.Commit();

        return await Task.FromResult(model.Id);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return 0;
      }
    }

    public async Task<int> CreateAutoJob(int scheduleId, EJobScheduleType jobType, EScheduleStatus status, DateTime scheduleTime)
    {
      try
      {
        var model = new JobScheduleModel
        {
          CreatedDate = DateTime.Now,
          IsExecuted = false,
          JobScheduleType = jobType,
          ScheduleStatus = status,
          ScheduleId = scheduleId,
          IsSchedule=true,
          ScheduleTime=scheduleTime
        };

        _jobScheduleRepository.Add(model);
        _unitOfWork.Commit();
        return await Task.Run(() => 1);
      }
      catch (Exception ex)
      {
        _logger.LogError("Create Auto Job:" + ex.Message);
        return await Task.Run(() => 0);
      }
    }

    public async Task<List<JobScheduleModel>> GetManualJobIsNotExecute()
    {
      var jobs = await _jobScheduleRepository.GetMultiJobAsync(x => x.IsExecuted==false && x.IsSchedule==false);
      return jobs;
    }

    public async Task<List<JobScheduleModel>> GetAutoJobIsNotExecute()
    {
      var jobs = await _jobScheduleRepository.GetMultiJobAsync(x => x.IsExecuted==false && x.IsSchedule==true);
      return jobs;
    }

    public async Task UpdateJobIsExecuted(int obScheduleId)
    {
      try
      {
        var job = await _jobScheduleRepository.GetSingleById_ExAsync(obScheduleId);
        job.IsExecuted = true;

      await  _jobScheduleRepository.UpdateStatusJobScheduleAsync(job);
      //  _unitOfWork.Commit();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
      }
    }
  }
}
