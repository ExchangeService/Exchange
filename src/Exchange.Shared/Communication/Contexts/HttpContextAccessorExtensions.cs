using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using Convey.MessageBrokers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Exchange.Shared.Communication.Contexts
{
    public static class HttpContextAccessorExtensions
    {
        public static CorrelationContext? GetCorrelationContext(this ICorrelationContextAccessor accessor)
        {
            if (accessor.CorrelationContext is null)
            {
                return null;
            }

            var payload = JsonSerializer.Serialize(accessor.CorrelationContext);

            return string.IsNullOrWhiteSpace(payload)
                       ? null
                       : JsonSerializer.Deserialize<CorrelationContext>(payload);
        }

        public static CorrelationContext? GetCorrelationContext(this IHttpContextAccessor accessor)
        {
            StringValues json = new StringValues();

            return accessor.HttpContext?.Request.Headers.TryGetValue("Correlation-Context", out json) is true
                       ? JsonSerializer.Deserialize<CorrelationContext>(json.FirstOrDefault() ?? string.Empty)
                       : null;
        }

        internal static IDictionary<string, object>? GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string SagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(SagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                       ? null
                       : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                         {
                             [SagaHeader] = saga
                         };
        }

        internal static string GetSpanContext(this IMessageProperties? messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
    }
}