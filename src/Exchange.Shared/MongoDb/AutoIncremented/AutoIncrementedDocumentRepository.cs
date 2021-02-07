using System;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace Exchange.Shared.MongoDb.AutoIncremented
{
    internal sealed class AutoIncrementedDocumentRepository : IAutoIncrementedDocumentRepository
    {
        private readonly IMongoDatabase database;

        public AutoIncrementedDocumentRepository(IMongoDatabase database) => this.database = database;

        public async ValueTask<long> GetNextSequenceValue(string tableName, string sequenceTableName)
        {
            var collection = this.database.GetCollection<SequenceDocument>(sequenceTableName);
            var filter = Builders<SequenceDocument>.Filter.Eq(a => a.Name, tableName);
            var update = Builders<SequenceDocument>.Update.Inc(a => a.Value, 1);
            update = update.SetOnInsert(
                "_id",
                Guid.NewGuid()
                    .ToString());

            var sequence = await collection.FindOneAndUpdateAsync(
                               filter,
                               update,
                               new FindOneAndUpdateOptions<SequenceDocument, SequenceDocument>
                               {
                                   IsUpsert = true,
                                   ReturnDocument = ReturnDocument.After
                               });

            return sequence.Value;
        }
    }
}