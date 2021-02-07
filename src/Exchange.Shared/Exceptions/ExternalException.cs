using System;

namespace Exchange.Shared.Exceptions
{
    public class ExternalException : Exception
    {
        public ExternalException(string? message, string? code)
            : base(message) =>
            this.Code = code;

        public string? Code { get; }
    }
}