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
            builder.Services.AddHostedService<MetricsJob>();
            builder.Services.AddSingleton<CustomMetricsMiddleware>();
            builder.Services.AddSingleton<IAppMetricProvider, AppMetricProvider>();

            return builder.AddMetrics();
        }

        public static void AddCommandMetric(this IConveyBuilder builder, string key, string command)
        {
            builder.Services.AddSingleton<IAppMetric>(
                new AppMetric(
                    new CounterOptions()
                    {
                        Name = "commands",
                        Tags = new MetricTags("command", command)
                    },
                    key));
        }

        public static void AddQueryMetric(this IConveyBuilder builder, string key, string query)
        {
            builder.Services.AddSingleton<IAppMetric>(
                new AppMetric(
                    new CounterOptions()
                    {
                        Name = "queries",
                        Tags = new MetricTags("query", query)
                    },
                    key));
        }

        public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder builder) =>
            builder.UseMetrics()
                .UseMiddleware<CustomMetricsMiddleware>();
    }
}