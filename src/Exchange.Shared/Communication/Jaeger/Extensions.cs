using Convey;
using Convey.CQRS.Commands;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Communication.Jaeger
{
    internal static class Extensions
    {
        public static IConveyBuilder AddJaegerDecorators(this IConveyBuilder builder)
        {
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(JaegerCommandHandlerDecorator<>));

            return builder;
        }
    }
}