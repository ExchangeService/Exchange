using System.Collections.Generic;
using System.Threading.Tasks;

using App.Metrics;
using App.Metrics.Counter;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using MetricsOptions = Convey.Metrics.AppMetrics.MetricsOptions;

namespace Exchange.Shared.Metrics
{
    public class CustomMetricsMiddleware : IMiddleware
    {
        private readonly bool enabled;

        private IDictionary<string, CounterOptions>? metrics;

        private readonly IServiceScopeFactory serviceScopeFactory;

        public CustomMetricsMiddleware(IServiceScopeFactory serviceScopeFactory, MetricsOptions options)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.enabled = options.Enabled;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!this.enabled)
            {
                return next(context);
            }

            this.metrics = context.RequestServices.GetService<IAppMetricProvider>()?
                               .Metrics ?? new Dictionary<string, CounterOptions>();

            var request = context.Request;
            if (!this.metrics.TryGetValue(GetKey(request.Method, request.Path.ToString()), out var metrics))
            {
                return next(context);
            }

            using var scope = this.serviceScopeFactory.CreateScope();
            var metricsRoot = scope.ServiceProvider.GetRequiredService<IMetricsRoot>();
            metricsRoot.Measure.Counter.Increment(metrics);

            return next(context);
        }


        private static string GetKey(string method, string path) => $"{method}:{path}";
    }
}