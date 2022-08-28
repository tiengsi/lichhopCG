using System;

namespace WebApi.Models.Dtos
{
    public class AuditScheduleDto
    {
        public int Id { get; set; }
        public string ChangeFrom { get; set; }
        public string ChangeTo { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ScheduleId { get; set; }      
    }
}
