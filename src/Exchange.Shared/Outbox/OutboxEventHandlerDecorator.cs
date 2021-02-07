using System;
using System.Threading.Tasks;

using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;

namespace Exchange.Shared.Outbox
{
    internal sealed class OutboxEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        private readonly bool enabled;

        private readonly IEventHandler<TEvent> handler;

        private readonly string messageId;

        private readonly IMessageOutbox outbox;

        public OutboxEventHandlerDecorator(
            IEventHandler<TEvent> handler,
            IMessageOutbox outbox,
            OutboxOptions outboxOptions,
            IMessagePropertiesAccessor messagePropertiesAccessor)
        {
            this.handler = handler;
            this.outbox = outbox;
            this.enabled = outboxOptions.Enabled;

            var messageProperties = messagePropertiesAccessor.MessageProperties;
            this.messageId = string.IsNullOrWhiteSpace(messageProperties?.MessageId)
                                 ? Guid.NewGuid().ToString("N")
                                 : messageProperties.MessageId;
        }

        public Task HandleAsync(TEvent @event) =>
            this.enabled
                ? this.outbox.HandleAsync(this.messageId, () => this.handler.HandleAsync(@event))
                : this.handler.HandleAsync(@event);
    }
}