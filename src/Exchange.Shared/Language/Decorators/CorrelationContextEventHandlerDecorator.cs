using System.Globalization;
using System.Threading.Tasks;

using Convey.CQRS.Events;

using Exchange.Shared.Communication.Contexts;

namespace Exchange.Shared.Language.Decorators
{
    internal sealed class CorrelationContextEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        private readonly IRequestContextAccessor contextAccessor;

        private readonly IEventHandler<TEvent> handler;

        public CorrelationContextEventHandlerDecorator(
            IEventHandler<TEvent> handler,
            IRequestContextAccessor contextAccessor)
        {
            this.handler = handler;
            this.contextAccessor = contextAccessor;
        }

        public Task HandleAsync(TEvent @event)
        {
            var context = this.contextAccessor.ReceivedContext;
            if (context is { })
            {
                HandleReceivedContext(context);
            }

            return this.handler.HandleAsync(@event);
        }

        private static void HandleReceivedContext(CorrelationContext context)
        {
            if (context.Language is { })
            {
                SetCurrentThreadCulture(context.Language);
            }
        }

        private static void SetCurrentThreadCulture(string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }
    }
}