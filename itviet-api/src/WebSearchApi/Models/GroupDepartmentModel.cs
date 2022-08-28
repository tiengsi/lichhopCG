using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("GroupDepartment")]
    public class GroupDepartmentModel
    {
        [Key, Column(Order = 0)]
        public int DepartmentId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupParticipantId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual DepartmentModel Department { get; set; }

        [ForeignKey("GroupParticipantId")]
        public virtual GroupParticipantModel GroupParticipant { get; set; }
    }
}
