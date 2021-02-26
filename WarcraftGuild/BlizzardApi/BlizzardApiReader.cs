using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Models;
using WarcraftGuild.Enums;
using WarcraftGuild.Exceptions;

namespace WarcraftGuild.BlizzardApi
{
    public class BlizzardApiReader : IBlizzardApiReader
    {
        private readonly IWebClient _webClient;
        private string _token;
        private DateTime _tokenExpiration;
        private BlizzardApiConfiguration ApiConfiguration;

        protected static BlizzardApiConfiguration DefaultConfig { get; set; }

        public static void SetDefaultConfiguration(BlizzardApiConfiguration configuration)
        {
            DefaultConfig = configuration;
        }

        public static void ClearDefaultConfiguration()
        {
            DefaultConfig = null;
        }

        public BlizzardApiConfiguration Configuration
        {
            get { return ApiConfiguration ?? DefaultConfig; }
            set
            {
                ApiConfiguration = value;
            }
        }

        public BlizzardApiReader(IOptions<BlizzardApiConfiguration> apiConfiguration, IWebClient webClient)
        {
            _webClient = webClient;
            Configuration = apiConfiguration.Value;
        }

        public async Task<string> GetJsonAsync(string query, Namespace? ns = null, string additionalParams = null)
        {
            ThrowIfInvalidRequest();
            if (HasTokenExpired())
                await SendTokenRequest();
            string urlRequest = ParsePath(query, ns, additionalParams);
            IApiResponse response = await _webClient.MakeApiRequestAsync(Configuration.GetApiUrl() + urlRequest);
            Configuration.NotifyAllLimits(this, response);
            switch (response.StatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    return json;
                default:
                    throw new BadResponseException($"Get JSon fail : {response.StatusCode()}", response.StatusCode(), response);
            }
        }

        public async Task<T> GetAsync<T>(string query, Namespace? ns = null, string additionalParams = null)
        {
            ThrowIfInvalidRequest();
            if (HasTokenExpired())
                await SendTokenRequest();
            string urlRequest = ParsePath(query, ns, additionalParams);
            IApiResponse response = await _webClient.MakeApiRequestAsync(Configuration.GetApiUrl() + urlRequest);
            Configuration.NotifyAllLimits(this, response);
            switch (response.StatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    return JsonSerializer.Deserialize<T>(json);
                default:
                    throw new BadResponseException($"Get {typeof(T)} fail : {response.StatusCode()}", response.StatusCode(), response);
            }
        }

        public async Task<string> SendTokenRequest()
        {
            var response = await _webClient.RequestAccessTokenAsync();
            switch (response.StatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    ClientCredentials credentials = JsonSerializer.Deserialize<ClientCredentials>(json);
                    _token = credentials.AccessToken;
                    int expiresInSeconds = credentials.ExpiresIn;
                    _tokenExpiration = DateTime.Now.AddSeconds(expiresInSeconds);
                    return _token;
                default:
                    throw new HttpRequestException($"Send Token Error : {response.StatusCode()}");
            }
        }

        private void ThrowIfInvalidRequest()
        {
            VerifyConfigurationIsValid();
            if (Configuration.AnyReachedLimit())
                throw new RateLimitReachedException("http request was blocked by RateLimiter");
        }

        private bool HasTokenExpired()
        {
            if (string.IsNullOrEmpty(_token)
                || DateTime.Now > _tokenExpiration)
                return true;
            return false;
        }

        private void VerifyConfigurationIsValid()
        {
            if (Configuration == null || _webClient == null)
                throw new NullReferenceException("ApiConfiguration is not set, either declare one as global configuration or set a local instance configuration object.");
        }

        private string ParsePath(string query, Namespace? ns = null, string addtionalParams = null)
        {
            string path = ParseSpecialCharacters(query.Trim())
                + "?locale=" + Configuration.GetLocaleString()
                + "&access_token=" + _token;
            if (ns.HasValue)
                path += $"&namespace={ns.Value.GetCode()}-{Configuration.GetRegionString().ToLower()}";
            if (!string.IsNullOrEmpty(addtionalParams))
                path += addtionalParams;
            return path;
        }

        private static string ParseSpecialCharacters(string s)
        {
            s = s.Replace("#", "%23");
            return s;
        }
    }
}