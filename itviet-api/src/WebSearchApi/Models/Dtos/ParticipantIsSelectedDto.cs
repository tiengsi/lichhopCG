using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class ParticipantIsSelectedDto
    {
        public string DepartmentName { get; set; }

        public int ParticipantId { get; set; }

        public string ReceiverName { get; set; }
    }

    public class ReceiverDto
    {
        public string PhoneNumber { get; set; }

        public string ReceiverName { get; set; }
    }
}
