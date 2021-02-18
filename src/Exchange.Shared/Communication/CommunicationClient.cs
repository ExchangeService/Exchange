using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Convey.HTTP;

using Exchange.Shared.Communication.Contexts;
using Exchange.Shared.Communication.Exceptions;
using Exchange.Shared.Exceptions;
using Exchange.Shared.Extensions;
using Exchange.Shared.Security;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;

using Polly;

namespace Exchange.Shared.Communication
{
    [UsedImplicitly]
    public class CommunicationClient: ICommunicationClient
    {
        private const string ContextHeaderName = "Correlation-Context";

        private readonly IHttpClient client;

        private readonly IRequestContextAccessor contextAccessor;

        private readonly HttpClientOptions options;

        private readonly IUserDetailsProvider userDetailsProvider;

        public CommunicationClient(
            IHttpClient client,
            IRequestContextAccessor contextAccessor,
            IUserDetailsProvider userDetailsProvider,
            HttpClientOptions options)
        {
            this.client = client;
            this.contextAccessor = contextAccessor;
            this.userDetailsProvider = userDetailsProvider;
            this.options = options;
        }

        public async Task<T> GetJsonAsync<T>(string uri, bool allowNull = false, bool relayHeaders = true)
            where T : class
        {
            if (relayHeaders)
            {
                await this.SetCorrelationContextAsync();
            }

            if (allowNull)
            {
                return await this.client.GetAsync<T>(uri);
            }

            return await Policy.Handle<ExternalException>()
                       .WaitAndRetryAsync(
                           this.options.Retries,
                           errorNumber => TimeSpan.FromMilliseconds(1000 * errorNumber))
                       .ExecuteAsync(
                           async () =>
                           {
                               var result = await this.client.GetAsync<T>(uri);
                               if (result is null)
                               {
                                   throw new CommunicationException("Result for this action could not be empty");
                               }

                               return result;
                           });
        }

        public async Task<OperationResult> SendJsonRequestAsync<T>(
            HttpMethod method,
            string uri,
            T request,
            bool save = true,
            bool relayHeaders = true)
            where T : class
        {
            if (relayHeaders)
            {
                await this.SetCorrelationContextAsync().WithoutCapturingContext();
            }

            var result = await SendRequest().WithoutCapturingContext();
            if (!result.IsSuccessStatusCode)
            {
                var stringResult = await result.Content.ReadAsStringAsync().WithoutCapturingContext();
                var jsonSerializerOptions = new JsonSerializerOptions
                              {
                                  PropertyNameCaseInsensitive = true
                              };
                if (stringResult.IsNullOrWhiteSpace())
                {
                    return save
                               ? OperationResult.Error(result.ReasonPhrase)
                               : throw new ExternalException(result.ReasonPhrase, "error");
                }

                var errorResponse = JsonSerializer.Deserialize<ExceptionDetails>(stringResult, jsonSerializerOptions);
                if (errorResponse is { })
                {
                    return save
                               ? OperationResult.Error(errorResponse.Reason, errorResponse.Code)
                               : throw new ExternalException(errorResponse.Reason, errorResponse.Code);
                }

                return save
                           ? OperationResult.Error(result.ReasonPhrase)
                           : throw new ExternalException(result.ReasonPhrase, "error");
            }

            return OperationResult.Success();

            Task<HttpResponseMessage> SendRequest()
            {
                if (method == HttpMethod.Post)
                {
                    return this.client.PostAsync(uri, request);
                }

                if (method == HttpMethod.Put)
                {
                    return this.client.PutAsync(uri, request);
                }

                if (method == HttpMethod.Delete)
                {
                    return this.client.DeleteAsync(uri);
                }

                if (method == HttpMethod.Patch)
                {
                    return this.client.PatchAsync(uri, request);
                }

                throw new InvalidOperationException($"Sending {method} not available");
            }
        }

        private async Task SetCorrelationContextAsync()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var context = this.contextAccessor.ContextForSend;
            if (context is { })
            {
                headers.Add(ContextHeaderName, JsonSerializer.Serialize(context));
            }

            await this.userDetailsProvider.RelayAuthorizationAsync(headers).WithoutCapturingContext();

            this.client.SetHeaders(headers);
        }
    }
}