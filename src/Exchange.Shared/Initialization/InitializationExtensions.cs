using Convey;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Initialization
{
    public static class InitializationExtensions
    {
        public static IConveyBuilder AddInitialization(this IConveyBuilder builder)
        {
            _ = builder.Services.AddHostedService<HostedInitializer>();
            return builder;
        }
    }
}