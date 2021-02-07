using Exchange.Shared.MongoDb.EventStore.Override;

using NEventStore;
using NEventStore.Serialization;
using NEventStore.Serialization.Bson;

namespace Exchange.Shared.MongoDb.EventStore
{
    internal sealed class EventStoreContext : IEventStoreContext
    {
        private readonly string connectionString;

        public EventStoreContext(string connectionString) => this.connectionString = connectionString;

        public IStoreEvents Events => WireupEventStore(this.connectionString);

        private static IStoreEvents WireupEventStore(string conn) =>
            Wireup.Init()
                .UsingAppMongoPersistence(conn, new DocumentObjectSerializer())
                .InitializeStorageEngine()
                .UsingBsonSerialization()
                .Compress()
                .Build();
    }
}