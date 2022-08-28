using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Jobs
{
  public class SendEmailSmsJob : IJob
  {
    private readonly ILogger<SendEmailSmsJob> _logger;
    private readonly IJobScheduleService _jobScheduleService;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IAuditScheduleRepository _auditScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendEmailSmsJob(
        ILogger<SendEmailSmsJob> logger,
        IJobScheduleService jobScheduleService,
        IEmailAndSmsService emailAndSmsService,
            IAuditScheduleRepository auditScheduleRepository,
        IUnitOfWork unitOfWork)
    {
      _logger = logger;
      _jobScheduleService = jobScheduleService;
      _emailAndSmsService = emailAndSmsService;
      _emailAndSmsService = emailAndSmsService;
      _auditScheduleRepository = auditScheduleRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
      try
      {
        _logger.LogInformation("Starting job....");

        var jobs = await _jobScheduleService.GetManualJobIsNotExecute();

        if (!jobs.Any())
        {
          _logger.LogWarning("Not found any job to execute!");
          return;
        }

        foreach (var job in jobs)
        {
          var isSuccess = true;
          if (job.JobScheduleType == Models.Enums.EJobScheduleType.EMAIL && job.ScheduleStatus == Models.Enums.EScheduleStatus.Approve)
          {
            _logger.LogInformation("Starting job EMAIL....");
            try
            {         
            isSuccess= await _emailAndSmsService.SendEmailForApprove(job.ScheduleId);
            }
            catch (Exception ex)
            {
             _logger.LogError("ERR job EMAIL..." + ex.Message + ex.StackTrace);
              throw;
            }
            if (isSuccess==true) SaveAudit(job.ScheduleId, "Đã gửi thư mời");
            _logger.LogInformation("End job EMAIL....");
          }
          else if (job.JobScheduleType == Models.Enums.EJobScheduleType.SMS && job.ScheduleStatus == Models.Enums.EScheduleStatus.Approve)
          {
            _logger.LogInformation("Starting job SMS....");
            try
            {

            isSuccess= await _emailAndSmsService.SendSmsForApprove(job.ScheduleId);

            }
            catch (Exception ex)
            {
              _logger.LogError("ERR job SMS..." + ex.Message + ex.StackTrace);
              throw;
            }
            if (isSuccess==true) SaveAudit(job.ScheduleId, "Đã gửi tin nhắn");
            _logger.LogInformation("End job SMS....");
          }

          await _jobScheduleService.UpdateJobIsExecuted(job.Id);
        }
      }
      catch (System.Exception ex)
      {
        _logger.LogError(ex.Message);
      }
    }

    private bool SaveAudit(int scheduleId, string message)
    {
      var auditSchedule = new AuditScheduleModel()
      {
        ChangeFrom = string.Empty,
        ChangeTo = message,
        ChangeDate = DateTime.Now,
        ScheduleId = scheduleId
      };
      _auditScheduleRepository.Add_Ex(auditSchedule);
      //_unitOfWork.Commit();
      return true;
    }
  }
}
