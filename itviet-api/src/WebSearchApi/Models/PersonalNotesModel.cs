using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("PersonalNotes")]
  public class PersonalNotesModel : CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int PersonalNotesId { get; set; }

    [Display(Name = "Tiêu đề ghi chú")]
    [Required(ErrorMessage = "Yêu cầu nhập tiêu đề !")]
    public string Title { get; set; }

    [Display(Name = "Người tạo")]
    public int UserId { get; set; }

    [Display(Name = "Nội dung")]
    [Column(TypeName = "ntext")]
    public string ContentNote { get; set; }

    public int? ScheduleId { get; set; }


    [ForeignKey("ScheduleId")]
    public virtual ScheduleModel ScheduleModel { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel User { get; set; }
  }
}
