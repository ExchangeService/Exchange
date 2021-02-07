using System.Globalization;

using Convey.MessageBrokers;

using Microsoft.AspNetCore.Http;

namespace Exchange.Shared.Communication.Contexts
{
    internal sealed class RequestContextAccessor : IRequestContextAccessor
    {
        private readonly ICorrelationContextAccessor contextAccessor;

        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestContextAccessor(
            ICorrelationContextAccessor contextAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            this.contextAccessor = contextAccessor;
            this.httpContextAccessor = httpContextAccessor;
        }

        public CorrelationContext? ContextForSend
        {
            get
            {
                var context = this.ReceivedContext;
                if (context is { })
                {
                    context.Language ??= CultureInfo.CurrentCulture.Name;
                }

                return context;
            }
        }

        public CorrelationContext? ReceivedContext =>
            this.contextAccessor.GetCorrelationContext() ?? this.httpContextAccessor.GetCorrelationContext();
    }
}