using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebApi.Helpers.Domains;

namespace WebApi.Helpers.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            _logger.LogError(ex, ex.Message);
            var response = new ApiResponse(context.Response.StatusCode, ex.Message);
            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex.InnerException != null)
            {
                _logger.LogError(ex, ex.InnerException.Message);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new ApiResponse(context.Response.StatusCode, "Đã có sự cố sảy ra! Hãy liên hệ với quản trị viên để được giúp đỡ");
            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
        }
    }
}
