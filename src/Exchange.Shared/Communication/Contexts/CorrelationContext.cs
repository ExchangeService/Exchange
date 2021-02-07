using System;
using System.Collections.Generic;

namespace Exchange.Shared.Communication.Contexts
{
    public sealed class CorrelationContext
    {
        public string? ConnectionId { get; set; }

        public string? CorrelationId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Language { get; set; }

        public string? Name { get; set; }

        public string? ResourceId { get; set; }

        public string? SpanContext { get; set; }

        public string? TraceId { get; set; }

        public UserContext? User { get; set; }

        public class UserContext
        {
            public IDictionary<string, string>? Claims { get; set; }

            public string? Id { get; set; }

            public bool? IsAuthenticated { get; set; }

            public string? Role { get; set; }
        }
    }
}