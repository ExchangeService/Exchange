using System;

namespace Exchange.Shared.Exceptions
{
    public class InvalidAggregateIdException : Exception
    {
        public InvalidAggregateIdException(object id)
            : base($"Invalid aggregate id: {id}") =>
            this.Id = id;

        public object Id { get; }
    }
}