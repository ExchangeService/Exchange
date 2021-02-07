using System;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Shared.Core
{
    public abstract class AggregateRoot<T> : IBaseAggregate<T>
        where T : IEquatable<T>
    {
        private readonly ISet<IDomainEvent> events = new HashSet<IDomainEvent>();

        protected AggregateRoot(AggregateId<T> id, int version)
        {
            this.Id = id;
            this.Version = version;
        }

        public IEnumerable<IDomainEvent> Events => this.events;

        public AggregateId<T> Id { get; set; }

        public int Version { get; set; }

        public void ClearEvents() => this.events.Clear();

        protected virtual void AddEvent(IDomainEvent @event)
        {
            if (!this.events.Any())
            {
                this.Version++;
            }

            this.events.Add(@event);
        }

        protected void AddEventToList(IDomainEvent @event) => this.events.Add(@event);
    }
}