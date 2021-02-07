using Convey.Persistence.MongoDB;

using JetBrains.Annotations;

namespace Exchange.Shared.MongoDb.EventStore
{
    [UsedImplicitly]
    internal sealed class MongoDbOptionsBuilder : IMongoDbOptionsBuilder
    {
        private readonly MongoDbOptions options = new();

        public MongoDbOptions Build() => this.options;

        public IMongoDbOptionsBuilder WithConnectionString(string connectionString)
        {
            this.options.ConnectionString = connectionString;
            return this;
        }

        public IMongoDbOptionsBuilder WithDatabase(string database)
        {
            this.options.Database = database;
            return this;
        }

        public IMongoDbOptionsBuilder WithSeed(bool seed)
        {
            this.options.Seed = seed;
            return this;
        }
    }
}