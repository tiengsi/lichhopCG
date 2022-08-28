using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data.Infrastructure
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(WebApiDbContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            context.Username = httpAccessor.HttpContext?.User.Identity.Name;
        }
    }
}
