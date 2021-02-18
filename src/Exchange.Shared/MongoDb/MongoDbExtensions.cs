using System;

using Convey;
using Convey.Persistence.MongoDB;

using Exchange.Shared.MongoDb.AutoIncremented;
using Exchange.Shared.MongoDb.Migration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson.Serialization.Conventions;

namespace Exchange.Shared.MongoDb
{
    public static class MongoDbExtensions
    {
        public static IConveyBuilder AddMongoDb(this IConveyBuilder builder, string migrationDbName)
        {
            _ = builder.Services
                .AddTransient<IMigrationRepository, MigrationRepository>()
                .AddTransient<IMigrator, Migrator>();

            var conventionPack = new ConventionPack
                                 {
                                     new CamelCaseElementNameConvention()
                                 };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);


            return builder
                .AddMongo()
                .AddMongoRepository<MigrationDocument, Guid>(migrationDbName)
                .AddAutoIncrementedRepository();
        }

        public static IApplicationBuilder UseMongoDb(this IApplicationBuilder builder)
        {
            var migrator = builder.ApplicationServices.GetService<IMigrator>();
            migrator?.MigrateAsync().GetAwaiter().GetResult();
            return builder;
        }
    }
}