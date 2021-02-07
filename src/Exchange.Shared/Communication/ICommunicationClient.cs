using System.Net.Http;
using System.Threading.Tasks;

namespace Exchange.Shared.Communication
{
    public interface ICommunicationClient
    {
        public Task<T> GetJsonAsync<T>(string uri, bool allowNull = false, bool relayHeaders = true)
            where T : class;

        public Task<OperationResult> SendJsonRequestAsync<T>(
            HttpMethod method,
            string uri,
            T request,
            bool save = true,
            bool relayHeaders = true)
            where T : class;
    }
}