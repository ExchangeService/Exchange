using NEventStore;

namespace Exchange.Shared.MongoDb.EventStore
{
    public interface IEventStoreContext
    {
        public IStoreEvents Events { get; }
    }
}