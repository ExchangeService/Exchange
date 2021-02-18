using Convey;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Initialization
{
    public static class InitializableSingletonExtensions
    {
        public static IConveyBuilder AddAsyncInitializableSingleton<TInitializer, TInterfaceInitializer>(
            this IConveyBuilder builder)
            where TInitializer : class, TInterfaceInitializer, ISingletonInitializerAsync
            where TInterfaceInitializer : class
        {
            _ = builder.Services
                .AddSingleton<TInitializer>()
                .AddSingleton<TInterfaceInitializer>(sp => sp.GetRequiredService<TInitializer>())
                .AddSingleton<ISingletonInitializerAsync>(sp => sp.GetRequiredService<TInitializer>());

            return builder;
        }

        public static IConveyBuilder AddInitializableSingleton<TInitializer, TInterfaceInitializer>(
            this IConveyBuilder builder)
            where TInitializer : class, TInterfaceInitializer, ISingletonInitializer
            where TInterfaceInitializer : class
        {
            _ = builder.Services
                .AddSingleton<TInitializer>()
                .AddSingleton<TInterfaceInitializer>(sp => sp.GetRequiredService<TInitializer>())
                .AddSingleton<ISingletonInitializer>(sp => sp.GetRequiredService<TInitializer>());

            return builder;
        }
    }
}