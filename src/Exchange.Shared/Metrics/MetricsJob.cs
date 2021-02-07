using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using App.Metrics;
using App.Metrics.Gauge;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MetricsOptions = Convey.Metrics.AppMetrics.MetricsOptions;

namespace Exchange.Shared.Metrics
{
    public class MetricsJob : BackgroundService
    {
        private readonly GaugeOptions threads = new GaugeOptions
        {
            Name = "threads"
        };

        private readonly GaugeOptions workingSet = new GaugeOptions
        {
            Name = "working_set"
        };

        private readonly ILogger<MetricsJob> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly MetricsOptions options;

        public MetricsJob(IServiceScopeFactory serviceScopeFactory, MetricsOptions options, ILogger<MetricsJob> logger)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            this.options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!this.options.Enabled)
            {
                this.logger.LogInformation("Metrics are disabled.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = this.serviceScopeFactory.CreateScope())
                {
                    this.logger.LogTrace("Processing metrics...");
                    var metricsRoot = scope.ServiceProvider.GetRequiredService<IMetricsRoot>();
                    var process = Process.GetCurrentProcess();
                    metricsRoot.Measure.Gauge.SetValue(this.threads, process.Threads.Count);
                    metricsRoot.Measure.Gauge.SetValue(this.workingSet, process.WorkingSet64);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}