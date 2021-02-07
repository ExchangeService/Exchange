using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;

using Exchange.Shared.Communication.Contexts;
using Exchange.Shared.Extensions;

using Microsoft.Extensions.Logging;

using OpenTracing;

namespace Exchange.Shared.Communication
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private const string DefaultSpanContextHeader = "span_context";

        private readonly IBusPublisher busPublisher;

        private readonly IRequestContextAccessor contextAccessor;

        private readonly ILogger<IMessageBroker> logger;

        private readonly IMessagePropertiesAccessor messagePropertiesAccessor;

        private readonly IMessageOutbox outbox;

        private readonly string spanContextHeader;

        private readonly ITracer tracer;

        public MessageBroker(
            IBusPublisher busPublisher,
            IMessageOutbox outbox,
            IRequestContextAccessor contextAccessor,
            IMessagePropertiesAccessor messagePropertiesAccessor,
            RabbitMqOptions options,
            ITracer tracer,
            ILogger<IMessageBroker> logger)
        {
            this.busPublisher = busPublisher;
            this.outbox = outbox;
            this.contextAccessor = contextAccessor;
            this.messagePropertiesAccessor = messagePropertiesAccessor;
            this.tracer = tracer;
            this.logger = logger;
            this.spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
                                         ? DefaultSpanContextHeader
                                         : options.SpanContextHeader;
        }

        public Task PublishAsync(params IEvent[] events) => this.PublishAsync(events?.AsEnumerable() ?? new List<IEvent>());

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            if (events is null)
            {
                return;
            }

            var messageProperties = this.messagePropertiesAccessor.MessageProperties;
            var originatedMessageId = messageProperties?.MessageId;
            var correlationId = messageProperties?.CorrelationId;
            var spanContext = messageProperties?.GetSpanContext(this.spanContextHeader);
            if (string.IsNullOrWhiteSpace(spanContext))
            {
                spanContext = this.tracer.ActiveSpan is null ? string.Empty : this.tracer.ActiveSpan.Context.ToString();
            }

            var headers = messageProperties?.GetHeadersToForward();
            var correlationContext = this.contextAccessor.ContextForSend;

            if (correlationContext is { })
            {
                correlationContext.Language = CultureInfo.CurrentCulture.Name;
            }

            foreach (var @event in events)
            {
                if (@event is null)
                {
                    continue;
                }

                var messageId = Guid.NewGuid().ToString("N");
                this.logger.LogTrace($"Publishing integration event: {@event.GetType().Name} [id: '{messageId}'].");
                if (this.outbox.Enabled)
                {
                    await this.outbox.SendAsync(
                            @event,
                            originatedMessageId,
                            messageId,
                            correlationId,
                            spanContext,
                            correlationContext,
                            headers)
                        .WithoutCapturingContext();
                    continue;
                }

                await this.busPublisher.PublishAsync(
                        @event,
                        messageId,
                        correlationId,
                        spanContext,
                        correlationContext,
                        headers)
                    .WithoutCapturingContext();
            }
        }
    }
}