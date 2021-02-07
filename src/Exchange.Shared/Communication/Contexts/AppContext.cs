using System;

namespace Exchange.Shared.Communication.Contexts
{
    internal sealed class AppContext : IAppContext
    {
        internal AppContext()
            : this(Guid.NewGuid().ToString("N"), IdentityContext.Empty)
        {
        }

        internal AppContext(CorrelationContext context)
            : this(
                context?.CorrelationId,
                context?.User is null ? IdentityContext.Empty : new IdentityContext(context.User))
        {
        }

        internal AppContext(string? requestId, IIdentityContext identity)
        {
            this.RequestId = requestId;
            this.Identity = identity;
        }

        public IIdentityContext? Identity { get; }

        public string? RequestId { get; }

        internal static IAppContext Empty => new AppContext();
    }
}