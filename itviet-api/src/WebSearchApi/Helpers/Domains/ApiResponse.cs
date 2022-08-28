using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace WebApi.Helpers.Domains
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return ReasonPhrases.GetReasonPhrase(statusCode);
        }
    }
}
