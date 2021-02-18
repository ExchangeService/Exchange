using App.Metrics;
using App.Metrics.Counter;

using Convey;
using Convey.Metrics.AppMetrics;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Metrics
{
    public static class MetricsExtensions
    {
        public static IConveyBuilder AddAppMetrics(this IConveyBuilder builder)
        {
            _ = builder.Services
                .AddHostedService<MetricsJob>()
                .AddSingleton<CustomMetricsMiddleware>()
                .AddSingleton<IAppMetricProvider, AppMetricProvider>();

            return builder.AddMetrics();
        }

        public static IConveyBuilder AddCommandMetric(this IConveyBuilder builder, string key, string command)
        {
            _ = builder.Services.AddSingleton<IAppMetric>(
                new AppMetric(
                    new CounterOptions()
                    {
                        Name = "commands",
                        Tags = new MetricTags("command", command)
                    },
                    key));

            return builder;
        }

        public static IConveyBuilder AddQueryMetric(this IConveyBuilder builder, string key, string query)
        {
            _ = builder.Services.AddSingleton<IAppMetric>(
                new AppMetric(
                    new CounterOptions()
                    {
                        Name = "queries",
                        Tags = new MetricTags("query", query)
                    },
                    key));

            return builder;
        }

        public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder builder) =>
            builder.UseMetrics()
                .UseMiddleware<CustomMetricsMiddleware>();
    }
}