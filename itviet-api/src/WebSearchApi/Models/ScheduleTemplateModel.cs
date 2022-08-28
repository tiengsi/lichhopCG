using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using WebApi.Models.Enums;

namespace WebApi.Models
{
  [Table("ScheduleTemplate")]
  public class ScheduleTemplateModel : CommonModel
  {
    public ScheduleTemplateModel()
    {
      ParticipantsModels = new HashSet<ParticipantsTemplateModel>();
      OtherParticipantModels = new HashSet<OtherParticipantTemplateModel>();
    }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduleId { get; set; }

    [Display(Name = "Giờ họp")]
    public string ScheduleTime { get; set; }

    [Display(Name = "Tiêu đề cuộc họp")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string ScheduleTitle { get; set; }

    [Display(Name = "Nội dung cuộc họp")]
    [AllowHtml]
    public string ScheduleContent { get; set; }

    [Display(Name = "Cán bộ chủ trì")]
    public int? Id { get; set; }

    [Display(Name = "Địa điểm")]
    public int? LocationId { get; set; }

    [Display(Name = "Địa điểm khác")]
    public string OtherLocation { get; set; }

    [Display(Name = "Người chủ trì khác")]
    public string OtherHost { get; set; }

    [Display(Name = "Thành phần tham gia")]
    public string Participants { get; set; }

    [Display(Name = "Hiển thị thành phần tham gia")]
    public string ParticipantDisplay { get; set; }

    [Display(Name = "Gửi Email")]
    public bool IsSendEmail { get; set; }

    [Display(Name = "Gửi SMS")]
    public bool ISendSMS { get; set; }

    [Display(Name = "Lý do dời lịch")]
    public string ReasonChangeSchedule { get; set; }

    [Display(Name = "Dời địa điểm")]
    public bool IsChangeLocation { get; set; }

    [Display(Name = "Mẫu tiêu đề")]
    public int? ScheduleTitleTemplateId { get; set; }
    public EScheduleStatus ScheduleStatus { get; set; }

    [Display(Name = "Nội dung tin nhắn")]
    public string MessageContent { get; set; }

    [Display(Name = "Các thành phần chuẩn bị")]
    public string DepartmentPrepare { get; set; }

    [Display(Name = "Đã gửi thư mời")]
    public bool IsSendSMSInvite { get; set; }

    [Display(Name = "Tài liệu đính kèm")]
    public string FilePath { get; set; }

    [Display(Name = "Cloudinary publicId")]
    public string CloudinaryPublicId { get; set; }

    [Display(Name = "OrganizeId")]
    public int OrganizeId { get; set; }

    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

    [ForeignKey("Id")]
    public virtual UserModel UserModel { get; set; }

    [ForeignKey("ScheduleTitleTemplateId")]
    public virtual ScheduleTitleTemplateModel ScheduleTitleTemplateModel { get; set; }

    [ForeignKey("LocationId")]
    public virtual LocationModel LocationModel { get; set; }

    public virtual ICollection<ParticipantsTemplateModel> ParticipantsModels { get; set; }
    public virtual ICollection<OtherParticipantTemplateModel> OtherParticipantModels { get; set; }
  }
}
