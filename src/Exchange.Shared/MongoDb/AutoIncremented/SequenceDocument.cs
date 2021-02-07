using System;

using Convey.Types;

using SmartAnalyzers.CSharpExtensions.Annotations;

namespace Exchange.Shared.MongoDb.AutoIncremented
{
    [InitRequired]
    public class SequenceDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public long Value { get; set; }
    }
}