namespace Exchange.Shared.Communication.Contexts
{
    public interface IRequestContextAccessor
    {
        CorrelationContext? ContextForSend { get; }

        CorrelationContext? ReceivedContext { get; }
    }
}