using System.Threading.Tasks;

namespace Exchange.Shared.MongoDb.Migration
{
    public interface IMigration
    {
        string Name { get; }

        int Version { get; }

        Task ExecuteAsync();
    }
}