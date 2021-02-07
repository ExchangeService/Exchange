namespace Exchange.Shared.Communication.Contexts
{
    internal interface IAppContextFactory
    {
        IAppContext Create();
    }
}