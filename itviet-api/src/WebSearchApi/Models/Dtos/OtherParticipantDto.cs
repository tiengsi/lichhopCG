namespace WebApi.Models.Dtos
{
    public class OtherParticipantDto
    {
        public int OtherParticipantId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int? ScheduleId { get; set; }
    }
}
