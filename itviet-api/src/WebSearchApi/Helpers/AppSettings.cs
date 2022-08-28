namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string TokenSecret { get; set; }

        public long MaxFileSize { get; set; }

        public string TokenExpiryDay { get; set; }

        public bool SSL { get; set; }

        public string AcceptedFileTypes { get; set; }
        public string HostURL { get; set; }
    }
}
