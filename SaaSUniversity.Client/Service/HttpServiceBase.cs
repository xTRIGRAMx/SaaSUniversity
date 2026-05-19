using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace SaaSUniversity.Client.Service
{
    public abstract class HttpServiceBase
    {
        protected readonly HttpClient Http;

        protected HttpServiceBase(HttpClient http) => Http = http;

        protected HttpRequestMessage CreateAuthenticatedRequest(HttpMethod method, string requestUri, object? content = null)
        {
            var request = new HttpRequestMessage(method, requestUri);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            if (content != null)
            {
                request.Content = JsonContent.Create(content);
            }

            return request;
        }
    }
}
