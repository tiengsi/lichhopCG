using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("ScheduledAttendances")]
  public class ScheduledAttendanceModel : CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduledAttendanceId { get; set; }

    [Display(Name = "Trạng thái tham dự")]
    public bool Status { get; set; }

    [Display(Name = "Thành phần tham dự")]
    public int UserId { get; set; }

    [Display(Name = "Thành phần khách mời")]
    public int OtherParticipantId { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel User { get; set; }
    [ForeignKey("OtherParticipantId")]
    public virtual OtherParticipantModel OtherUser { get; set; }
  }
}
