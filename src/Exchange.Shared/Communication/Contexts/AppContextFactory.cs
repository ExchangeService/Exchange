using Convey.MessageBrokers;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Exchange.Shared.Communication.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        private readonly ICorrelationContextAccessor contextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.contextAccessor = contextAccessor;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IAppContext Create()
        {
            if (this.contextAccessor.CorrelationContext is {})
            {
                var payload = JsonConvert.SerializeObject(this.contextAccessor.CorrelationContext);
                
                var handledContext = string.IsNullOrWhiteSpace(payload)
                    ? AppContext.Empty
                    : new AppContext(JsonConvert.DeserializeObject<CorrelationContext>(payload));

                return handledContext;
            }

            var context = this.httpContextAccessor.GetCorrelationContext();
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return context is null ? AppContext.Empty : new AppContext(context);
        }
    }
}