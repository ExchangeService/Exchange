using Convey;

using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.MongoDb.AutoIncremented
{
    public static class Extensions
    {
        public static IConveyBuilder AddAutoIncrementedRepository(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IAutoIncrementedDocumentRepository, AutoIncrementedDocumentRepository>();

            return builder;
        }
    }
}