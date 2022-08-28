using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("ScheduledResultDocuments")]
  public class ScheduledResultDocumentModel : CommonModel
  {

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduledResultDocumentId { get; set; }

    [Display(Name = "Tên tài liệu")]
    [Required(ErrorMessage = "Yêu cầu nhập tiêu đề !")]
    public string Title { get; set; }

    [Display(Name = "Trạng thái")]
    public bool Status { get; set; }

    [Display(Name = "Đường dẫn lưu trữ")]
    public string Path { get; set; }

    [Display(Name = "Thời gian cập nhập")]
    public DateTime DocumentUpdatedDate { get; set; }

    public int? ScheduleId { get; set; }


    [ForeignKey("ScheduleId")]
    public virtual ScheduleModel ScheduleModel { get; set; }
  }
}
