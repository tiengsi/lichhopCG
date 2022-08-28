using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("EmailLogs")]
    public class EmailLogsModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool SendEmailIsSuccess { set; get; }

        public bool SendSmsIsSuccess { set; get; }

        public DateTime SendDate { get; set; }

        public int? UserId { get; set; }

        public int? ScheduleId { get; set; }

        public int? OtherParticipantId { get; set; }

        public bool IsSmsLog { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual ScheduleModel Schedule { get; set; }

        [ForeignKey("OtherParticipantId")]
        public virtual OtherParticipantModel OtherParticipant { get; set; }
    }
}
