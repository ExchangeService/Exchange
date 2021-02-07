using App.Metrics.Counter;

namespace Exchange.Shared.Metrics
{
    public interface IAppMetric
    {
        public CounterOptions CounterOptions { get; }

        public string Key { get; }
    }
}