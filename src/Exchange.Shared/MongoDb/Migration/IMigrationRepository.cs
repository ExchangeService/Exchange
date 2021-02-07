using System.Threading.Tasks;

namespace Exchange.Shared.MongoDb.Migration
{
    public interface IMigrationRepository
    {
        public Task AddMigrationAsync(int version, string name);

        public Task<int> GetMaxVersionAsync();
    }
}