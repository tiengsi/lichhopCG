using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.Jobs
{
  public class CleanFileJob : IJob
  {
    private readonly IScheduleService _scheduleService;
    private readonly IOrganizeService _organizeService;
    private readonly IScheduleFilesAttachmentService _scheduleFilesAttachmentService;
    private readonly Helpers.UploadSettings _uploadSettings;
    ILogger<UploaderService> _logger;
    public CleanFileJob(IScheduleService scheduleService, IScheduleFilesAttachmentService scheduleFilesAttachmentService, IOptions<Helpers.UploadSettings> uploadSettings, IOrganizeService organizeService, ILogger<UploaderService> logger)
    {
      _scheduleService = scheduleService;
      _scheduleFilesAttachmentService = scheduleFilesAttachmentService;
      _uploadSettings=uploadSettings.Value;
      _organizeService=organizeService;
      _logger=logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {

      try
      {


        var allSchedule = await _scheduleService.GetAllScheduleByDateAsync(DateTime.Now.Date.AddDays(-7));
        foreach (var schedule in allSchedule)
        {

          var scheduleFileAttachmentList = await _scheduleFilesAttachmentService.getAllFilesAttachmentByScheduleIdAsync_Ex(schedule.ScheduleId);

          foreach (var fileAttachment in scheduleFileAttachmentList)
          {
            var deletePath = $"{Directory.GetCurrentDirectory()}\\Upload\\{fileAttachment.FilePath.Replace(_uploadSettings.Schedule, nameof(_uploadSettings.Schedule))}";
            if (File.Exists(deletePath))
            {
              // If file found, delete it    
              File.Delete(deletePath);
              _logger.LogInformation($"Clean File Attachment Task Schedule: Deleted {deletePath}");
              var deleteResult = await _scheduleFilesAttachmentService.DeleteFileAttachmentByIdAsync(fileAttachment.Id);
            }
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogInformation("Error at CleanFileJob: "+ex.ToString());
        return;
      }
      return;
    }

  }
}

