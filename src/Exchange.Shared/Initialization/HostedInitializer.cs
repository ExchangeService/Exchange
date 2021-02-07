using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Exchange.Shared.Initialization
{
    [UsedImplicitly]
    internal sealed class HostedInitializer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public HostedInitializer(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.serviceProvider.CreateScope();

            var initializers = scope.ServiceProvider.GetServices<ISingletonInitializer>()
                .ToList();
            initializers.ForEach(e => e.Initialize());

            using var scope2 = this.serviceProvider.CreateScope();

            var asyncInitializers = scope2.ServiceProvider.GetServices<ISingletonInitializerAsync>()
                .ToList();

            foreach (var initializer in asyncInitializers)
            {
                await initializer.InitializeAsync();
            }
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}