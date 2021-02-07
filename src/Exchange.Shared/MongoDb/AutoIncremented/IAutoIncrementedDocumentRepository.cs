using System.Threading.Tasks;

namespace Exchange.Shared.MongoDb.AutoIncremented
{
    public interface IAutoIncrementedDocumentRepository
    {
        ValueTask<long> GetNextSequenceValue(string tableName, string sequenceTableName = "Sequences");
    }
}