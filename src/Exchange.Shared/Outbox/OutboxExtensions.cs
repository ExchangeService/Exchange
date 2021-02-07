using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Outbox
{
    public static class OutboxExtensions
    {
        public static IConveyBuilder AddOutbox(this IConveyBuilder builder)
        {
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            return builder
                .AddMessageOutbox(o => o.AddMongo());
        }
    }
}