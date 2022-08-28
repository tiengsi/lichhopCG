using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    [Table("ScheduleHistory")]
    public class ScheduleHistoryModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ScheduleHistoryId { get; set; }

        public int ScheduleId { get; set; }

        public DateTime ScheduleDate { get; set; }

        public string ScheduleTime { get; set; }

        [Display(Name = "Giờ họp")]
        [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
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

        [Display(Name = "Version No")]
        public int VersionNo { get; set; }

        [Display(Name = "Gửi Email")]
        public bool IsSendEmail { get; set; }

        [Display(Name = "Gửi SMS")]
        public bool ISendSMS { get; set; }

        [ForeignKey("Id")]
        public virtual UserModel UserModel { get; set; }

        [ForeignKey("LocationId")]
        public virtual LocationModel LocationModel { get; set; }
    }
}
