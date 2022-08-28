using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class ScheduledResultDocumentDto
  {
    public int ScheduledResultDocumentId { get; set; }
    public string Title { get; set; }
    public bool Status { get; set; }
    public string Path { get; set; }
    public DateTime DocumentUpdatedDate { get; set; }
    public int? ScheduleId { get; set; }

    public ScheduledResultDocumentModel ToModel()
    {
      var model = new ScheduledResultDocumentModel()
      {

        ScheduleId = ScheduleId,
        Title = Title,
        Status = Status,
        Path = Path,
        CreatedBy = "System",
        UpdatedBy = "System",
        UpdatedDate = DateTime.Now,
        DocumentUpdatedDate = DocumentUpdatedDate
      };
      return model;
    }
  }
}
