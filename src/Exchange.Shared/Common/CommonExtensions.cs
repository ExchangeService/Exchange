using Convey;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Common
{
    public static class CommonExtensions
    {
        public static IConveyBuilder AddCommon(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<IFileReader, FileReader>();
            builder.Services.AddSingleton<IRandomTextGenerator, RandomTextGenerator>();

            return builder;
        }
    }
}