using System;
using System.Collections.Generic;

using Exchange.Shared.Core;

namespace Exchange.Shared.MongoDb.EventStore
{
    public interface IEventBaseAggregate<T> : IBaseAggregate<T>
        where T : IEquatable<T>
    {
        void ApplyEvent(IDomainEvent @event);

        void ClearUncommittedEvents();

        IEnumerable<IDomainEvent> GetUncommittedEvents();

        void LoadFromHistory(IEnumerable<ISyncDomainEvent> historyEvents);

        void NewEvent(ISyncDomainEvent @event);
    }
}