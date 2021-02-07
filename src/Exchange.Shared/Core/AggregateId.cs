using System;

using Exchange.Shared.Exceptions;

namespace Exchange.Shared.Core
{
    public class AggregateId<T> : IEquatable<AggregateId<T>>
        where T : IEquatable<T>
    {
        public AggregateId(T value)
        {
            if (value.Equals(default))
            {
                throw new InvalidAggregateIdException(value);
            }

            this.Value = value;
        }

        public T Value { get; }

        public static implicit operator T(AggregateId<T> id) => id.Value;

        public static implicit operator AggregateId<T>(T id) => new(id);

        public bool Equals(AggregateId<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || this.Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((AggregateId<T>)obj);
        }

        public override int GetHashCode() => this.Value.GetHashCode();

        public override string ToString() => this.Value?.ToString() ?? string.Empty;
    }
}