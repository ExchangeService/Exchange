using System.Collections.Generic;

using App.Metrics.Counter;

namespace Exchange.Shared.Metrics
{
    public interface IAppMetricProvider
    {
        IDictionary<string, CounterOptions> Metrics { get; }
    }
}