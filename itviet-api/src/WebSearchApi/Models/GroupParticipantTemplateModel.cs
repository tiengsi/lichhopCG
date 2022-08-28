using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("GroupParticipantTemplate")]
    public class GroupParticipantTemplateModel
    {
        public GroupParticipantTemplateModel()
        {
            GroupDepartmentModels = new HashSet<GroupDepartmentTemplateModel>();
            UserParticipantTemplateModels = new HashSet<UserParticipantTemplateModel>();
            OtherParticipantTemplateModels = new HashSet<OtherParticipantTemplateModel>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupParticipantId { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Yêu cầu nhập tên !")]
        [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự !")]
        public string GroupParticipantName { get; set; }

        public virtual ICollection<GroupDepartmentTemplateModel> GroupDepartmentModels { get; set; }

        public virtual ICollection<UserParticipantTemplateModel> UserParticipantTemplateModels { get; set; }

        public virtual ICollection<OtherParticipantTemplateModel> OtherParticipantTemplateModels { get; set; }

    }

    [Table("GroupDepartmentTemplate")]
    public class GroupDepartmentTemplateModel
    {
        [Key, Column(Order = 0)]
        public int DepartmentId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupParticipantId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual DepartmentTemplateModel Department { get; set; }

        [ForeignKey("GroupParticipantId")]
        public virtual GroupParticipantTemplateModel GroupParticipant { get; set; }
    }

    [Table("UserParticipantTemplate")]
    public class UserParticipantTemplateModel
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupParticipantId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        [ForeignKey("GroupParticipantId")]
        public virtual GroupParticipantTemplateModel GroupParticipant { get; set; }
    }

}
