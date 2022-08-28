using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("User")]
  public class UserModel : IdentityUser<int>
  {
    public UserModel()
    {
      UserRoles = new HashSet<UserRoleModel>();
      ScheduleModels = new HashSet<ScheduleModel>();
      ScheduleHistoryModels = new HashSet<ScheduleHistoryModel>();
      RepresentativeModels = new HashSet<RepresentativeModel>();
      ParticipantsModels = new HashSet<ParticipantsModel>();
      UserParticipantModels = new HashSet<UserParticipantModel>();
    }

    [MaxLength(256)]
    public string FullName { set; get; }

    [MaxLength(256)]
    public string ShortName { set; get; }

    // Là người chủ trì
    public bool IsHost { set; get; }

    // Hiện/Ẩn danh bạn
    public bool IsShow { set; get; } = true;

    public DateTime LastLogin { get; set; }

    public DateTime? UpdatedDate { set; get; }

    public int? DptId { get; set; }

    [MaxLength(500)]
    public string OfficerPosition { get; set; }

    [MaxLength(256)]
    public string UpdatedBy { set; get; }

    public virtual ICollection<UserRoleModel> UserRoles { get; set; }

    public virtual ICollection<ScheduleModel> ScheduleModels { get; set; }

    public virtual ICollection<ScheduleHistoryModel> ScheduleHistoryModels { get; set; }

    public virtual ICollection<RepresentativeModel> RepresentativeModels { get; set; }

    [ForeignKey("DptId")]
    public virtual DepartmentModel Department { get; set; }

    public virtual ICollection<ParticipantsModel> ParticipantsModels { get; set; }

    public virtual ICollection<UserParticipantModel> UserParticipantModels { get; set; }

    public virtual ICollection<ScheduleTemplateModel> ScheduleTemplateModels { get; set; }

    public virtual ICollection<RepresentativeTemplateModel> RepresentativeTemplateModels { get; set; }

    [ForeignKey("DptTempId")]
    public virtual DepartmentTemplateModel DepartmentTemplate { get; set; }
    [Display(Name = "Mã organizeId")]
    public int? OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

    public virtual ICollection<ParticipantsTemplateModel> ParticipantsTemplateModels { get; set; }

    public virtual ICollection<UserParticipantTemplateModel> UserParticipantTemplateModels { get; set; }
  }
}
