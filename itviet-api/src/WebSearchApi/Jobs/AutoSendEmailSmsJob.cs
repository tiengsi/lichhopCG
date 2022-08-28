using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Jobs
{
  public class AutoSendEmailSmsJob : IJob
  {
    private readonly ILogger<SendEmailSmsJob> _logger;
    private readonly IJobScheduleService _jobScheduleService;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IAuditScheduleRepository _auditScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AutoSendEmailSmsJob(
        ILogger<SendEmailSmsJob> logger,
        IJobScheduleService jobScheduleService,
        IEmailAndSmsService emailAndSmsService,
        IUnitOfWork unitOfWork,
        IAuditScheduleRepository auditScheduleRepository)
    {
      _logger = logger;
      _jobScheduleService = jobScheduleService;
      _emailAndSmsService = emailAndSmsService;
      _auditScheduleRepository = auditScheduleRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
      try
      {
        _logger.LogInformation("Starting Auto job....");
        var scheduleTime = DateTime.Now;
        DateTime date1WithoutSeconds = new DateTime(scheduleTime.Year, scheduleTime.Month, scheduleTime.Day, scheduleTime.Hour, scheduleTime.Minute, 0);
        

        var jobs = await _jobScheduleService.GetAutoJobIsNotExecute();

        if (!jobs.Any())
        {
          _logger.LogWarning("Not found any auto job to execute!");
          return;
        }
        var auditSchedules = new List<AuditScheduleModel>();
        var isSkip = true;
        foreach (var job in jobs)
        {
          DateTime date2WithoutSeconds = new DateTime(job.ScheduleTime.Value.Year, job.ScheduleTime.Value.Month, job.ScheduleTime.Value.Day, job.ScheduleTime.Value.Hour, job.ScheduleTime.Value.Minute, 0);
          if (job.JobScheduleType == Models.Enums.EJobScheduleType.EMAIL && job.ScheduleStatus == Models.Enums.EScheduleStatus.Approve && date1WithoutSeconds.Equals(date2WithoutSeconds)==true)
          {
            _logger.LogInformation("AutoJob: Starting job EMAIL....");
            await _emailAndSmsService.SendEmailForApprove(job.ScheduleId);
            _logger.LogInformation("AutoJob: End job EMAIL....");

            var auditSchedule = new AuditScheduleModel()
            {
              ChangeFrom = string.Empty,
              ChangeTo = "Đã gửi thư mời",
              ChangeDate = DateTime.Now,
              ScheduleId = job.ScheduleId
            };
            auditSchedules.Add(auditSchedule);
            isSkip=false;
          }
          else if (job.JobScheduleType == Models.Enums.EJobScheduleType.SMS && job.ScheduleStatus == Models.Enums.EScheduleStatus.Approve && date1WithoutSeconds.Equals(date2WithoutSeconds)==true)
          {
            _logger.LogInformation("AutoJob: Starting job SMS....");
            await _emailAndSmsService.SendSmsForApprove(job.ScheduleId);
            _logger.LogInformation("AutoJob: End job SMS....");

            var auditSchedule = new AuditScheduleModel()
            {
              ChangeFrom = string.Empty,
              ChangeTo = "Đã gửi tin nhắn",
              ChangeDate = DateTime.Now,
              ScheduleId = job.ScheduleId
            };
            auditSchedules.Add(auditSchedule);
            isSkip=false;
          }
          if (isSkip==false)
          {
            await _jobScheduleService.UpdateJobIsExecuted(job.Id);
            _auditScheduleRepository.AddMulti_Ex(auditSchedules);
            _unitOfWork.Commit();
          }
        }
      }
      catch (System.Exception ex)
      {
        _logger.LogError(ex.Message);
      }
    }
  }
}
