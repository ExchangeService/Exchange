namespace Exchange.Shared.Communication
{
    public interface IAppContext
    {
        IIdentityContext? Identity { get; }

        string? RequestId { get; }
    }
}