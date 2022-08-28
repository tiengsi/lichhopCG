using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Settings;
using WebApi.Services;

namespace WebApi
{
  public class APIPermissionAuthenticationMiddleware
  {
    private readonly RequestDelegate _next;  

    public APIPermissionAuthenticationMiddleware(RequestDelegate next)
    {
      _next = next;     
    }

    // IMessageWriter is injected into InvokeAsync
    public async Task InvokeAsync(HttpContext httpContext, IPermissionService permissionService)
    {
 
      int userId = 0;

      Endpoint endpointBeingHit = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
      ControllerActionDescriptor actionDescriptor = endpointBeingHit?.Metadata?.GetMetadata<ControllerActionDescriptor>();
      var routeTemp = actionDescriptor?.AttributeRouteInfo?.Template;
      var method = httpContext.Request.Method;

      if (routeTemp==null)
      {
        httpContext.Response.StatusCode = 404;
        return;
      }

      if (Constants.API_Not_Check.Any(x => x.Key==method && x.Value==routeTemp)==true)
      {
        await _next(httpContext);
        return;
      }

      if (httpContext.Request.Headers.Any(x => x.Key=="UserId")==false)
      {
        httpContext.Response.StatusCode = 403;
        return;
      }
      var userIdStr = httpContext.Request.Headers.Where(x => x.Key=="UserId").FirstOrDefault().Value;
      var isUserId = int.TryParse(userIdStr, out userId);
      if (isUserId==false)
      {
        httpContext.Response.StatusCode = 403;
        return;
      }

      try
      {       
        var result = await permissionService.CheckPermisisonAccessAPIByUserId(userId, routeTemp, method);
        if (result==false)
        {
          httpContext.Response.StatusCode = 403;
          return;
        }
      }
      catch (Exception ex)
      {
        httpContext.Response.StatusCode = 403;
        await httpContext.Response.WriteAsync(ex.ToString());
        return;
      }
      await _next(httpContext);
    }
  }

  public static class APIPermissionAuthenticationMiddlewareExtensions
  {
    public static IApplicationBuilder UseAPIPermissionAuthenticationMiddleware(
        this IApplicationBuilder builder)
    {
      return  builder.UseMiddleware<APIPermissionAuthenticationMiddleware>();
    }
  }
}
