using System.Reflection;

using Convey;
using Convey.Logging.CQRS;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Logging
{
    public static class LoggingExtensions
    {
        public static IConveyBuilder AddLogging<T>(
            this IConveyBuilder builder,
            Assembly assembly,
            T logTemplateMapper)
            where T : class, IMessageToLogTemplateMapper
        {
            _ = builder.Services.AddSingleton<IMessageToLogTemplateMapper>(logTemplateMapper);

            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}