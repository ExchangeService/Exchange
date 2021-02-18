using Convey;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Common
{
    public static class CommonExtensions
    {
        public static IConveyBuilder AddCommon(this IConveyBuilder builder)
        {
            _ = builder.Services
                .AddMemoryCache()
                .AddSingleton<IFileReader, FileReader>()
                .AddSingleton<IRandomTextGenerator, RandomTextGenerator>()
                .AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return builder;
        }
    }
}