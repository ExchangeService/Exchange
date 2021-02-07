using System.Threading.Tasks;

namespace Exchange.Shared.Initialization
{
    public interface ISingletonInitializerAsync
    {
        Task InitializeAsync();
    }
}