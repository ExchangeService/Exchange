using System.Globalization;
using System.Threading.Tasks;

using Convey.CQRS.Queries;

using Exchange.Shared.Communication.Contexts;

namespace Exchange.Shared.Language.Decorators
{
    internal sealed class CorrelationContextQueryHandlerDecorator<TQuery, TDto> : IQueryHandler<TQuery, TDto>
        where TQuery : class, IQuery<TDto>
    {
        private readonly IRequestContextAccessor contextAccessor;

        private readonly IQueryHandler<TQuery, TDto> handler;

        public CorrelationContextQueryHandlerDecorator(
            IQueryHandler<TQuery, TDto> handler,
            IRequestContextAccessor contextAccessor)
        {
            this.handler = handler;
            this.contextAccessor = contextAccessor;
        }

        public Task<TDto> HandleAsync(TQuery query)
        {
            var context = this.contextAccessor.ReceivedContext;
            if (context is { })
            {
                HandleReceivedContext(context);
            }

            return this.handler.HandleAsync(query);
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