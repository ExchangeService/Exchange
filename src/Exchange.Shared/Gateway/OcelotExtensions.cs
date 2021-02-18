using Convey;
using Convey.Auth;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Types;
using Convey.WebApi;

using Exchange.Shared.Exceptions;
using Exchange.Shared.Gateway.Infrastructure;
using Exchange.Shared.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Exchange.Shared.Gateway
{
    public static class OcelotExtensions
    {
        public static IConveyBuilder AddGatewayInfrastructure(this IConveyBuilder builder, IConfiguration configuration)
        {
            _ = builder.Services
                .AddSingleton<IPayloadBuilder, PayloadBuilder>()
                .AddSingleton<ICorrelationContextBuilder, CorrelationContextBuilder>()
                .AddTransient<ResourceIdGeneratorMiddleware>();

            return builder
                .AddErrorHandler<DefaultExceptionToResponseMapper>()
                .AddJaeger()
                .AddJwt()
                .AddSecurity()
                .AddWebApi()
                .AddOcelotInfrastructure(configuration);
        }
        
        public static IApplicationBuilder UseGatewayInfrastructure(this IApplicationBuilder app)
        {
            _ = app.UseRabbitMq();

            return app
                .UseConvey()
                .UseErrorHandler()
                .UseAuth()
                .UseOcelotInfrastructure()
                .MapWhen(
                    ctx => ctx.Request.Path == "/",
                    a =>
                    {
                        _ = a.Use(
                            (ctx, next) =>
                            {
                                var appOptions = ctx.RequestServices.GetRequiredService<AppOptions>();
                                return ctx.Response.WriteAsync(appOptions.Name);
                            });
                    }); ;
        }

        public static IConveyBuilder AddOcelotInfrastructure(this IConveyBuilder builder, IConfiguration configuration)
        {
            _ = builder.Services.AddOcelot()
                .AddConsul()
                .AddPolly()
                .AddDelegatingHandler<CorrelationContextHandler>(true);

            _ = builder.Services.AddSwaggerForOcelot(configuration);

            return builder;
        }

        public static IApplicationBuilder UseOcelotInfrastructure(this IApplicationBuilder app)
        {
            _ = app.UseMiddleware<ResourceIdGeneratorMiddleware>()
                .UseOcelot(GetOcelotConfiguration())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return app.UseSwaggerForOcelotUI(
                opt => {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });
        }

        private static OcelotPipelineConfiguration GetOcelotConfiguration() =>
            new()
            {
                AuthenticationMiddleware = async (context, next) =>
                {
                    var authenticateResult = await context.AuthenticateAsync()
                                                 .ConfigureAwait(false);
                    if (authenticateResult.Succeeded && authenticateResult.Principal is {})
                    {
                        context.User = authenticateResult.Principal;
                        await next.Invoke();
                        return;
                    }

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            };
    }
}