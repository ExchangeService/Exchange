using System;

namespace Exchange.Shared.Common
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        
        DateTime UtcNow { get; }
    }
}