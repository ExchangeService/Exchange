using System.Collections.Generic;
using System.Reflection;

using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi.CQRS;

using Exchange.Shared.Communication.Contexts;
using Exchange.Shared.Communication.Jaeger;
using Exchange.Shared.Communication.Routing;
using Exchange.Shared.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Communication
{
    internal static class CommunicationExtensions
    {
        public static IConveyBuilder AddCommunication(
            this IConveyBuilder builder,
            Assembly eventsHandlerAssembliesToScan,
            List<string> documentationAssemblies,
            string serviceName)
        {
            _ = builder.Services
                .AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>()
                    .Create())
                .AddTransient<IAppContextFactory, AppContextFactory>()
                .AddScoped<IRequestContextAccessor, RequestContextAccessor>()
                .AddTransient<ICommunicationClient, CommunicationClient>()
                .AddTransient<IMessageBroker, MessageBroker>()
                .AddTransient<IEventProcessor, EventProcessor>()
                .AddTransient<IEventProcessor, EventProcessor>()
                .Scan(
                    s => s.FromAssemblies(eventsHandlerAssembliesToScan)
                        .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());

            _ = builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(JaegerCommandHandlerDecorator<>));

            return builder
                .AddQueryHandlers()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryQueryDispatcher()
                .AddInMemoryEventDispatcher()
                .AddInMemoryCommandDispatcher()
                .AddHttpClient()
                .AddHttpRouting(documentationAssemblies, serviceName)
                .AddConsul()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddFabio()
                .AddJaeger();
        }

        public static IApplicationBuilder UseCommunication(this IApplicationBuilder builder)
        {
            _ = builder.UseRabbitMq();

            return builder.UseHttpRouting()
                .UsePublicContracts<ContractAttribute>()
                .UseJaeger()
                .UseConvey();
        }
    }
}