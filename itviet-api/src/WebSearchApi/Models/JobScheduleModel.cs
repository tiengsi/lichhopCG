using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Enums;

namespace WebApi.Models
{
    [Table("JobSchedule")]
    public class JobScheduleModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public EJobScheduleType JobScheduleType { get; set; }

        public bool IsExecuted { get; set; }

        public EScheduleStatus ScheduleStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsSchedule { get; set; }
        public DateTime? ScheduleTime { get; set; }
    }
}
