using System;
using System.Collections.Generic;

namespace Exchange.Shared.Communication.Contexts
{
    internal sealed class IdentityContext : IIdentityContext
    {
        internal IdentityContext()
        {
        }

        internal IdentityContext(CorrelationContext.UserContext context)
            : this(context.Id, context.Role, context.IsAuthenticated ?? false, context.Claims)
        {
        }

        internal IdentityContext(string? id, string? role, bool isAuthenticated, IDictionary<string, string>? claims)
        {
            this.Id = Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
            this.Role = role ?? string.Empty;
            this.IsAuthenticated = isAuthenticated;
            this.IsAdmin = this.Role.Equals("admin", StringComparison.InvariantCultureIgnoreCase);
            this.Claims = claims ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, string> Claims { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Guid Id { get; }

        public bool IsAdmin { get; }

        public bool IsAuthenticated { get; }

        public string Role { get; } = string.Empty;

        internal static IIdentityContext Empty => new IdentityContext();
    }
}