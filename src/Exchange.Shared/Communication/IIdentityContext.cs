using System;
using System.Collections.Generic;

namespace Exchange.Shared.Communication
{
    public interface IIdentityContext
    {
        IDictionary<string, string> Claims { get; }

        Guid Id { get; }

        bool IsAdmin { get; }

        bool IsAuthenticated { get; }

        string Role { get; }
    }
}