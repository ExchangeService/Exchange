using System;

namespace Exchange.Shared.Core
{
    public interface IBaseAggregate<T>
        where T : IEquatable<T>
    {
        public AggregateId<T> Id { get; set; }
    }
}