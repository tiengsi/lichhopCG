using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("ScheduledResultReports")]
  public class ScheduledResultReportModel : CommonModel
  {

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduledResultReportId { get; set; }



    [Display(Name = "Người báo cáo")]
    public int UserId { get; set; }

    [Display(Name = "Đường dẫn lưu trữ")]
    public string Path { get; set; }

    [Display(Name = "Nội dung báo cáo")]
    public string ReportContent { get; set; }

    [Display(Name = "Thời gian báo cáo")]
    public DateTime ReportTime { get; set; }

    public int? ScheduleId { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel User { get; set; }

    [ForeignKey("ScheduleId")]
    public virtual ScheduleModel ScheduleModel { get; set; }
  }
}
