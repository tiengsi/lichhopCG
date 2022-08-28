using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("PersonalSchedules")]
  public class PersonalScheduleModel : CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int PersonalScheduleId { get; set; }

    [Display(Name = "Tiêu đề")]
    [Required(ErrorMessage = "Yêu cầu nhập tiêu đề !")]
    public string Title { get; set; }

    [Display(Name = "Người tạo")]
    public int UserId { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Thời gian từ")]
    public DateTime Fromdate { get; set; }

    [Display(Name = "Thời gian đến")]
    public DateTime ToDate { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel User { get; set; }
  }
}
