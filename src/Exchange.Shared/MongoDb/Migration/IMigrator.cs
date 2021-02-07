using System.Threading.Tasks;

namespace Exchange.Shared.MongoDb.Migration
{
    public interface IMigrator
    {
        Task MigrateAsync();
    }
}