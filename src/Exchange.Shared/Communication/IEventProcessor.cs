using System.Collections.Generic;
using System.Threading.Tasks;

using Exchange.Shared.Core;

namespace Exchange.Shared.Communication
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent>? events);
    }
}