using System.Threading.Tasks;

namespace Exchange.Shared.Core
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}