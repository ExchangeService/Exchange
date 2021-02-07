using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace Exchange.Shared.Gateway.Infrastructure
{
    internal static class ApplicationBuilderExtensions
    {
        private const string LanguageHeader = "Language";

        private const string OperationHeader = "X-Operation";

        private const string ResourceIdKey = "resource-id";

        public static string? GetLanguage(this HttpContext request) =>
            request.Request.Headers.TryGetValue(LanguageHeader, out var language) ? language[0] : null;

        public static string? GetResourceIdFoRequest(this HttpContext context) =>
            context.Items.TryGetValue(ResourceIdKey, out var id) ? id as string : string.Empty;

        public static void SetOperationHeader(this HttpResponse response, string id) =>
            response.Headers.Add(OperationHeader, $"operations/{id}");

        public static void SetResourceIdFoRequest(this HttpContext context, string id) =>
            context.Items.TryAdd(ResourceIdKey, id);
    }
}