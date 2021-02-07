using System;
using System.Threading.Tasks;

using Convey.Persistence.MongoDB;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Exchange.Shared.MongoDb.Migration
{
    internal sealed class MigrationRepository : IMigrationRepository
    {
        private readonly IMongoRepository<MigrationDocument, Guid> repository;

        public MigrationRepository(IMongoRepository<MigrationDocument, Guid> repository) =>
            this.repository = repository;

        public Task AddMigrationAsync(int version, string name) =>
            this.repository.AddAsync(
                new MigrationDocument
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Version = version
                });

        public async Task<int> GetMaxVersionAsync()
        {
            var any = await this.repository.FindAsync(e => true);
            if (any.Count == 0)
            {
                return 0;
            }

            var maxVersion = await this.repository.Collection.AsQueryable().MaxAsync(e => e.Version);
            return maxVersion;
        }
    }
}