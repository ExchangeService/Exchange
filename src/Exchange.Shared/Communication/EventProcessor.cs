using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Convey.CQRS.Events;

using Exchange.Shared.Core;
using Exchange.Shared.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Exchange.Shared.Communication
{
    internal sealed class EventProcessor : IEventProcessor
    {
        private readonly IEventMapper eventMapper;

        private readonly ILogger<IEventProcessor> logger;

        private readonly IMessageBroker messageBroker;

        private readonly IServiceScopeFactory serviceScopeFactory;

        public EventProcessor(
            IServiceScopeFactory serviceScopeFactory,
            IEventMapper eventMapper,
            IMessageBroker messageBroker,
            ILogger<IEventProcessor> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.eventMapper = eventMapper;
            this.messageBroker = messageBroker;
            this.logger = logger;
        }

        public async Task ProcessAsync(IEnumerable<IDomainEvent>? events)
        {
            if (events is null)
            {
                return;
            }

            this.logger.LogTrace("Processing domain events...");
            var integrationEvents = await this.HandleDomainEventsAsync(events);
            if (!integrationEvents.Any())
            {
                return;
            }

            this.logger.LogTrace("Processing integration events...");
            await this.messageBroker.PublishAsync(integrationEvents).WithoutCapturingContext();
        }

        private async Task<List<IEvent>> HandleDomainEventsAsync(IEnumerable<IDomainEvent> events)
        {
            var integrationEvents = new List<IEvent>();
            using var scope = this.serviceScopeFactory.CreateScope();
            foreach (var @event in events)
            {
                var eventType = @event.GetType();
                this.logger.LogTrace($"Handling domain event: {eventType.Name}");
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
                dynamic handlers = scope.ServiceProvider.GetServices(handlerType);
                foreach (var handler in handlers)
                {
                    _ = await handler.HandleAsync((dynamic)@event);
                }

                var integrationEvent = this.eventMapper.Map(@event);
                if (integrationEvent is null)
                {
                    continue;
                }

                integrationEvents.Add(integrationEvent);
            }

            return integrationEvents;
        }
    }
}