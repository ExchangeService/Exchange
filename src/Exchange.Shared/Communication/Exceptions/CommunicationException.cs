using System;

namespace Exchange.Shared.Communication.Exceptions
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message)
            : base(message)
        {
        }

        public CommunicationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}