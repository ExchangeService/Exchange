using System;

using Exchange.Shared.Core;

namespace Exchange.Shared.MongoDb.EventStore
{
    public interface ISyncDomainEvent : IDomainEvent
    {
        public DateTime Time { get; }
    }
}