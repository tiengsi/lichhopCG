using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class SendSmsDto
    {
        public List<string> PhoneNumber { get; set; }

        public string Content { get; set; }
        public int OrganizeId { get; set; }
    }

    public class SmsResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string MessageId { get; set; }
    }
}
