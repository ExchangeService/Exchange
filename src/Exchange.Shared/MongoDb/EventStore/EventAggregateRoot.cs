using System;
using System.Collections.Generic;
using System.Linq;

using Exchange.Shared.Core;

using JetBrains.Annotations;

namespace Exchange.Shared.MongoDb.EventStore
{
    public abstract class EventAggregateRoot<T> : AggregateRoot<T>, IEventBaseAggregate<T>
        where T : IEquatable<T>, new()
    {
        private readonly ICollection<IDomainEvent> uncommittedEvents = new LinkedList<IDomainEvent>();

        protected EventAggregateRoot([NotNull] AggregateId<T> id, int version)
            : base(id, version)
        {
        }

        public abstract void Apply(IDomainEvent @event);

        public void ApplyEvent(IDomainEvent @event)
        {
            this.Version++;
            this.Apply(@event);
        }

        public void ClearUncommittedEvents() => this.uncommittedEvents.Clear();

        public IEnumerable<IDomainEvent> GetUncommittedEvents() => this.uncommittedEvents;

        public void LoadFromHistory(IEnumerable<ISyncDomainEvent> historyEvents)
        {
            var events = historyEvents.ToList()
                .OrderBy(e => e.Time);

            foreach (var domainEvent in events)
            {
                this.ApplyEvent(domainEvent);
            }
        }

        public void NewEvent(ISyncDomainEvent @event) => this.AddEvent(@event);

        protected override void AddEvent(IDomainEvent @event)
        {
            this.AddEventToList(@event);

            this.ApplyEvent(@event);
            this.uncommittedEvents.Add(@event);
        }
    }
}