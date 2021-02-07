using System.Collections.Generic;

using Convey.CQRS.Events;

using Exchange.Shared.Core;

namespace Exchange.Shared.Communication
{
    public interface IEventMapper
    {
        IEvent? Map(IDomainEvent @event);

        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}