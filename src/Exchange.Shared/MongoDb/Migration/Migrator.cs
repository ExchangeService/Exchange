using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

namespace Exchange.Shared.MongoDb.Migration
{
    [UsedImplicitly]
    public class Migrator : IMigrator
    {
        private readonly ILogger<Migrator> logger;

        private readonly IMigrationRepository migrationRepository;

        private readonly IEnumerable<IMigration> migrations;

        public Migrator(
            IEnumerable<IMigration> migrations,
            IMigrationRepository migrationRepository,
            ILogger<Migrator> logger)
        {
            this.migrations = migrations.ToList();
            this.migrationRepository = migrationRepository;
            this.logger = logger;
        }

        public async Task MigrateAsync()
        {
            var version = await this.migrationRepository.GetMaxVersionAsync().ConfigureAwait(false);

            this.logger.LogInformation($"Current max migration version {version}");

            var migrationsToRun = this.migrations.Where(e => e.Version > version).OrderBy(e => e.Version).ToList();

            foreach (var migration in migrationsToRun)
            {
                this.logger.LogInformation($"Execute migration {migration.Name} with version {migration.Version}");
                await migration.ExecuteAsync();

                await this.migrationRepository.AddMigrationAsync(migration.Version, migration.Name)
                    .ConfigureAwait(false);
            }

            this.logger.LogInformation(!migrationsToRun.Any() ? "No required migrations to run" : "Migration finished");
        }
    }
}