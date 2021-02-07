using Exchange.Shared.Communication.Contexts;

using Microsoft.AspNetCore.Http;

namespace Exchange.Shared.Gateway.Infrastructure
{
    internal interface ICorrelationContextBuilder
    {
        CorrelationContext Build(
            HttpContext context,
            string correlationId,
            string spanContext,
            string? language,
            string? name = null,
            string? resourceId = null);
    }
}