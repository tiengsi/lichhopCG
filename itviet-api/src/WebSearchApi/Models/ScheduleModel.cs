using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using WebApi.Models.Enums;

namespace WebApi.Models
{
  [Table("Schedule")]
  public class ScheduleModel : CommonModel
  {
    public ScheduleModel()
    {
      ParticipantsModels = new HashSet<ParticipantsModel>();
      OtherParticipantModels = new HashSet<OtherParticipantModel>();
      AuditSchedules = new HashSet<AuditScheduleModel>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduleId { get; set; }

    public DateTime ScheduleDate { get; set; }

    [Display(Name = "Giờ họp")]
    public string ScheduleTime { get; set; }
    
    public DateTime? ScheduleEndDate { get; set; }

    [Display(Name = "Giờ kết thúc họp")]
    public string ScheduleEndTime { get; set; }

    [Display(Name = "Tiêu đề cuộc họp")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string ScheduleTitle { get; set; }

    [Display(Name = "Nội dung cuộc họp")]
    [AllowHtml]
    public string ScheduleContent { get; set; }

    [Display(Name = "Cán bộ chủ trì")]
    public int? Id { get; set; }

    [Display(Name = "Lãnh đạo tham dự")]
    public string IncludedOfficer { get; set; }

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

    [Display(Name = "Đường dẫn họp online")]
    public string MeetingLink { get; set; }

    [Display(Name = "Gửi email tự động hay không")]
    public bool IsAutoSendAtScheduledTime { get; set; }=false;

    [Display(Name = "BrandNameId")]
    public int? BrandNameId { get; set; }


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

    public virtual ICollection<ParticipantsModel> ParticipantsModels { get; set; }

    public virtual ICollection<OtherParticipantModel> OtherParticipantModels { get; set; }
    public virtual ICollection<AuditScheduleModel> AuditSchedules { get; set; }
    public virtual ICollection<ScheduleFilesAttachmentModel> ScheduleFilesAttachment { get; set; }

  }

}
