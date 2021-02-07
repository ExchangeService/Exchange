using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Exchange.Shared.Gateway.Infrastructure
{
    internal sealed class ResourceIdGeneratorMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }
            
            var resourceId = Guid.NewGuid().ToString("N");
            context.SetResourceIdFoRequest(resourceId);
            await next(context);
        }
    }
}