using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("OtherParticipant")]
    public class OtherParticipantModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OtherParticipantId { get; set; }

        [Display(Name = "Tên đơn vị/ họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề !")]
        [StringLength(300, ErrorMessage = "Bạn chỉ được nhập tối đa 300 ký tự !")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public int? ScheduleId { get; set; }

        [Display(Name = "Cán bộ ngoài hệ thống")]
        public int? GroupParticipantId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual ScheduleModel ScheduleModel { get; set; }

        [ForeignKey("GroupParticipantId")]
        public virtual GroupParticipantModel GroupParticipantModel { get; set; }
    }
}
