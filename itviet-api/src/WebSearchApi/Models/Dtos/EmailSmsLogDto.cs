using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class EmailSmsLogDto
    {
        public bool SendSmsIsSuccess { get; set; }

        public bool SendEmailIsSuccess { get; set; }

        public string FullName { get; set; }

        public string DepartmentName { get; set; }

        public bool IsOtherPariticipant { get; set; }
    }

    public class EmailSmsStatusDto
    {
        public List<EmailSmsLogDto> EmailSmsLogs { get; set; }

        public bool IsCompleteSendEmail { get; set; }

        public bool IsCompleteSendSms { get; set; }
    }
}
