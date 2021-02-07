using System;
using System.Linq;
using System.Threading.Tasks;

using Convey.CQRS.Commands;

using OpenTracing;
using OpenTracing.Tag;

namespace Exchange.Shared.Communication.Jaeger
{
    internal sealed class JaegerCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> handler;

        private readonly ITracer tracer;

        public JaegerCommandHandlerDecorator(ICommandHandler<TCommand> handler, ITracer tracer)
        {
            this.handler = handler;
            this.tracer = tracer;
        }

        public async Task HandleAsync(TCommand command)
        {
            var commandName = ToUnderscoreCase(command.GetType().Name);
            using var scope = this.BuildScope(commandName);
            var span = scope.Span;

            try
            {
                span.Log($"Handling a message: {commandName}");
                await this.handler.HandleAsync(command);
                span.Log($"Handled a message: {commandName}");
            }
            catch (Exception ex)
            {
                span.Log(ex.Message);
                span.SetTag(Tags.Error, true);
                throw;
            }
        }

        private static string ToUnderscoreCase(string str) =>
            string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLowerInvariant();

        private IScope BuildScope(string commandName)
        {
            var scope = this.tracer.BuildSpan($"handling-{commandName}").WithTag("message-type", commandName);

            if (this.tracer.ActiveSpan is { })
            {
                scope.AddReference(References.ChildOf, this.tracer.ActiveSpan.Context);
            }

            return scope.StartActive(true);
        }
    }
}