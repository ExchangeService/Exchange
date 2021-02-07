using System.Collections.Generic;
using System.Reflection;

using Convey;
using Convey.Logging.CQRS;
using Convey.Persistence.Redis;
using Convey.WebApi;
using Convey.WebApi.Exceptions;

using Exchange.Shared.Communication;
using Exchange.Shared.Initialization;
using Exchange.Shared.Language;
using Exchange.Shared.Logging;
using Exchange.Shared.Metrics;
using Exchange.Shared.MongoDb;
using Exchange.Shared.Outbox;
using Exchange.Shared.Security;

using Microsoft.AspNetCore.Builder;

namespace Exchange.Shared
{
    public static class SharedExtensions
    {
        public static IConveyBuilder AddShared<TLogger, TExceptionToResponseMapper>(
            this IConveyBuilder builder,
            string serviceName,
            string defaultLanguage,
            List<string> supportingLanguages,
            Assembly applicationAssembliesToScan,
            string migrationDbName,
            TLogger logger,
            List<string> documentationAssemblies)
            where TLogger : class, IMessageToLogTemplateMapper
            where TExceptionToResponseMapper : class, IExceptionToResponseMapper =>
            builder.AddCommunication(applicationAssembliesToScan, documentationAssemblies, serviceName)
                .AddErrorHandler<TExceptionToResponseMapper>()
                .AddLogging(applicationAssembliesToScan, logger)
                .AddAuth()
                .AddMongoDb(migrationDbName)
                .AddRedis()
                .AddOutbox()
                .AddLanguage(defaultLanguage, supportingLanguages)
                .AddInitialization()
                .AddAppMetrics();

        public static IApplicationBuilder UseShared(this IApplicationBuilder builder) =>
            builder.UseErrorHandler()
                .UseConvey()
                .UseCommunication()
                .UseAuth()
                .UseLanguage()
                .UseMongoDb()
                .UseAppMetrics();
    }
}