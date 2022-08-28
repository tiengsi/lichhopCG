using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  // Nhóm người tham dự
  [Table("GroupParticipant")]
  public class GroupParticipantModel : CommonModel
  {
    public GroupParticipantModel()
    {
      GroupDepartmentModels = new HashSet<GroupDepartmentModel>();
      UserParticipantModels = new HashSet<UserParticipantModel>();
      OtherParticipantModels = new HashSet<OtherParticipantModel>();
    }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int GroupParticipantId { get; set; }

    [Display(Name = "Tên nhóm")]
    [Required(ErrorMessage = "Yêu cầu nhập tên !")]
    [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự !")]
    public string GroupParticipantName { get; set; }

    public virtual ICollection<GroupDepartmentModel> GroupDepartmentModels { get; set; }

    public virtual ICollection<UserParticipantModel> UserParticipantModels { get; set; }

    public virtual ICollection<OtherParticipantModel> OtherParticipantModels { get; set; }

    [Display(Name = "Mã organizeId")]
    public int OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

  }
}
