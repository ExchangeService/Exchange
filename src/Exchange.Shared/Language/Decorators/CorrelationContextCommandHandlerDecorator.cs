using System.Globalization;
using System.Threading.Tasks;

using Convey.CQRS.Commands;

using Exchange.Shared.Communication.Contexts;

namespace Exchange.Shared.Language.Decorators
{
    internal sealed class CorrelationContextCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IRequestContextAccessor contextAccessor;

        private readonly ICommandHandler<TCommand> handler;

        public CorrelationContextCommandHandlerDecorator(
            ICommandHandler<TCommand> handler,
            IRequestContextAccessor contextAccessor)
        {
            this.handler = handler;
            this.contextAccessor = contextAccessor;
        }

        public Task HandleAsync(TCommand command)
        {
            var context = this.contextAccessor.ReceivedContext;
            if (context is { })
            {
                HandleReceivedContext(context);
            }

            return this.handler.HandleAsync(command);
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