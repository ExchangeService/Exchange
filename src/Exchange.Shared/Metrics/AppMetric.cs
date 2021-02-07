using App.Metrics.Counter;

namespace Exchange.Shared.Metrics
{
    internal sealed class AppMetric : IAppMetric
    {
        public AppMetric(CounterOptions counterOptions, string key)
        {
            this.CounterOptions = counterOptions;
            this.Key = key;
        }

        public CounterOptions CounterOptions { get; }

        public string Key { get; }
    }
}