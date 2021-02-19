using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Models;
using WarcraftGuild.Enums;

namespace WarcraftGuild.BlizzardApi.Configuration
{
    public class ApiWebClient : IWebClient
    {
        private readonly HttpClient _apiClient;
        private readonly HttpClient _authClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiWebClient(IHttpClientFactory httpClientFactory, IOptions<BlizzardApiConfiguration> apiConfiguration)
        {
            var _configuration = apiConfiguration.Value;

            _httpClientFactory = httpClientFactory;

            authPath = _configuration.GetAuthUrl();

            _apiClient = ConfigureApiClient();
            _authClient = ConfigureAuthClient(AuthenticateHeader(_configuration.ClientId, _configuration.ClientSecret));

        }


        private readonly FormUrlEncodedContent _authRequestContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                });

        private readonly string authPath;

        public async Task<IApiResponse> MakeApiRequestAsync(string path)
        {
            var response = await _apiClient.GetAsync(path);
            return new ApiResponse(response);
        }

        public async Task<IApiResponse> RequestAccessTokenAsync()
        {

            FormUrlEncodedContent _authRequestContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                });

            var response = await _authClient.PostAsync(authPath, _authRequestContent);
            return new ApiResponse(response);
        }

        private HttpClient ConfigureApiClient()
        {
            var client = _httpClientFactory.CreateClient(NamedHttpClients.ApiClient.ToString());
            return client;
        }

        private HttpClient ConfigureAuthClient(AuthenticationHeaderValue header)
        {
            var client = _httpClientFactory.CreateClient(NamedHttpClients.AuthClient.ToString());

            client.DefaultRequestHeaders.Authorization = header;

            return client;
        }

        private static AuthenticationHeaderValue AuthenticateHeader(string clientId, string clientSecret)
        {
            return new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(clientId + ":" + clientSecret)));
        }
    }
}
