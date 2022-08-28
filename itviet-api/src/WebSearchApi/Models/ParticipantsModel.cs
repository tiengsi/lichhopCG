using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Participants")]
    public class ParticipantsModel
    {
        [Key, Column(Order = 0)]
        public int ScheduleId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual ScheduleModel Schedule { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
     
    [Table("ParticipantsTemplate")]
    public class ParticipantsTemplateModel
    {
        [Key, Column(Order = 0)]
        public int ScheduleId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual ScheduleTemplateModel Schedule { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
}
