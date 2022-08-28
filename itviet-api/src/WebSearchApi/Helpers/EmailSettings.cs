namespace WebApi.Helpers
{
    public class EmailSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class SmsSettings
    {
        public string Username { get; set; }

        public string Phonenumber { get; set; }

        public string ContentType { get; set; }

        public string Password { get; set; }

        public string BrandName { get; set; }
        public string Provider { get; set; }
        public string CPCode { get; set; }
        public string LinkURL { get; set; }
    }
}
