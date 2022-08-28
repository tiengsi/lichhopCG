using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class ScheduledResultReportDto
  {
    public int ScheduledResultReportId { get; set; }
    public int UserId { get; set; }
    public string Path { get; set; }
    public string ReportContent { get; set; }
    public DateTime ReportTime { get; set; }
    public int? ScheduleId { get; set; }

    public ScheduledResultReportModel ToModel()
    {
      var model = new ScheduledResultReportModel()
      {    
        ScheduleId = ScheduleId,
        UserId = UserId,
        ReportTime = ReportTime,
        Path = Path,
        CreatedBy = UserId.ToString(),       
        UpdatedBy = UserId.ToString(),
        UpdatedDate = DateTime.Now,
        ReportContent = ReportContent
      };
      return model;
    }
  }
}
