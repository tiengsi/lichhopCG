using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
    public class ParticipantForSelectDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public EParticipantType ParticipantType { get; set; }

        public string Id { get; set; }
    }
}
