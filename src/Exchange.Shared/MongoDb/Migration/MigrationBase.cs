using System;
using System.Threading.Tasks;

namespace Exchange.Shared.MongoDb.Migration
{
    public abstract class MigrationBase : IMigration
    {
        protected readonly IServiceProvider Provider;

        protected MigrationBase(IServiceProvider provider) => this.Provider = provider;

        public abstract string Name { get; }

        public abstract int Version { get; }

        public abstract Task ExecuteAsync();
    }
}