using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Convey;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Exchange.Shared.Communication.Routing
{
    public static class RoutingExtensions
    {
        public static IConveyBuilder AddDocumentation(this IConveyBuilder builder, List<string> documentationAssemblies, string serviceName)
        {
            _ = builder.Services.AddSwaggerGen(
                c =>
                {
                    c.DescribeAllParametersInCamelCase();
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = serviceName,
                            Version = "v1"
                        });
                    c.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                        {
                            Description =
                                "JWT Authorization header using the Bearer scheme. Example Authorization: Bearer {token}",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        });

                    var securityScheme = new OpenApiSecurityScheme
                                         {
                                             Reference = new OpenApiReference
                                                         {
                                                             Type = ReferenceType.SecurityScheme,
                                                             Id = "Bearer"
                                                         }
                                         };

                    c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            [securityScheme] = new List<string>()
                        });

                    foreach (var serviceFilePath in documentationAssemblies.Select(assembly => Path.Combine(AppContext.BaseDirectory, assembly)))
                    {
                        c.IncludeXmlComments(serviceFilePath);
                    }
                });

            return builder;
        }

        public static IConveyBuilder AddHttpRouting(this IConveyBuilder builder, List<string> documentationAssemblies, string serviceName)
        {
            _ = builder.Services
                .AddControllers();

            _ = builder.Services.AddMvc(options => { })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(_ => { });

            return builder.AddDocumentation(documentationAssemblies, serviceName);
        }

        public static IApplicationBuilder UseDocumentation(this IApplicationBuilder builder)
        {
            return builder.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
        }

        public static IApplicationBuilder UseHttpRouting(this IApplicationBuilder app) =>
            app.UseRouting()
                .UseEndpoints(
                    endpoints =>
                    {
                        _ = endpoints.MapControllers();
                    })
                .UseDocumentation();
    }
}