using System;

namespace Exchange.Shared.Core
{
    public interface IAuditable
    {
        public bool IsDeleted { get; }

        public string ModifiedBy { get; }

        public DateTime ModifiedOn { get; }
    }
}