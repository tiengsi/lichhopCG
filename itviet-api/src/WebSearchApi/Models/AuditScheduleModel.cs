using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("AuditSchedule")]
    public class AuditScheduleModel: CommonModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ChangeFrom { get; set; }
        public string ChangeTo { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public virtual ScheduleModel Schedule { get; set; }
    }
}
