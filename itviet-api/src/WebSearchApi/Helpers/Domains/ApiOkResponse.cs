using Microsoft.AspNetCore.Http;

namespace WebApi.Helpers.Domains
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public bool IsSuccess { get; set; }

        public ApiOkResponse(object result, int statusCode = StatusCodes.Status200OK, bool isSuccess = true, string message = null) : base(statusCode, message)
        {
            Result = result;
            IsSuccess = isSuccess;
        }
    }
}
