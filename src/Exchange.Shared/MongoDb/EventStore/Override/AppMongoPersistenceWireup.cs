using System;
using System.Transactions;

using Microsoft.Extensions.Logging;

using NEventStore;
using NEventStore.Logging;
using NEventStore.Serialization;

namespace Exchange.Shared.MongoDb.EventStore.Override
{
    internal sealed class AppMongoPersistenceWireup : PersistenceWireup
    {
        private static readonly ILogger Logger = LogFactory.BuildLogger(typeof(MongoPersistenceWireup));

        public AppMongoPersistenceWireup(
            Wireup inner,
            Func<string> connectionStringProvider,
            IDocumentSerializer serializer,
            AppMongoPersistenceOptions? persistenceOptions)
            : base(inner)
        {
            Logger.LogDebug("Configuring Mongo persistence engine.");
            if (this.Container.Resolve<TransactionScopeOption>() != TransactionScopeOption.Suppress)
            {
                Logger.LogWarning("MongoDB does not participate in transactions using TransactionScope.");
            }

            this.Container.Register(
                _ => new AppMongoPersistenceFactory(connectionStringProvider, serializer, persistenceOptions).Build());
        }
    }
}