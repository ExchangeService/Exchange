using System.Collections.Generic;
using System.Linq;

using App.Metrics.Counter;

namespace Exchange.Shared.Metrics
{
    internal sealed class AppMetricProvider : IAppMetricProvider
    {
        public AppMetricProvider(IEnumerable<IAppMetric> metrics) =>
            this.Metrics = metrics.ToDictionary(e => e.Key, e => e.CounterOptions);

        public IDictionary<string, CounterOptions> Metrics { get; }
    }
}