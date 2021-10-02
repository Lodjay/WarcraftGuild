using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Models;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Exceptions;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.BlizzardApi
{
    public class BlizzardApiReader : IBlizzardApiReader
    {
        private readonly IWebClient _webClient;
        private readonly BlizzardApiConfiguration _config;
        private string _token;
        private DateTime _tokenExpiration;

        public BlizzardApiReader(IOptions<BlizzardApiConfiguration> apiConfiguration, IWebClient webClient)
        {
            _webClient = webClient;
            _config = apiConfiguration.Value;
        }

        public async Task<string> GetJsonAsync(string query, Namespace? ns = null, string additionalParams = null)
        {
            ThrowIfInvalidRequest();
            if (HasTokenExpired())
                await SendTokenRequest();
            string urlRequest = ParsePath(query, ns, additionalParams);
            IApiResponse response = await _webClient.MakeApiRequestAsync(_config.GetApiUrl() + urlRequest);
            _config.NotifyAllLimits();
            switch (response.GetStatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    return json;

                default:
                    throw new BadResponseException($"Get JSON fail : {response.GetStatusCode()}", response.GetStatusCode(), response);
            }
        }

        public async Task<T> GetAsync<T>(string query, Namespace? ns = null, string additionalParams = null)
        {
            ThrowIfInvalidRequest();
            if (HasTokenExpired())
                await SendTokenRequest();
            string urlRequest = ParsePath(query, ns, additionalParams);
            IApiResponse response = await _webClient.MakeApiRequestAsync(_config.GetApiUrl() + urlRequest);
            _config.NotifyAllLimits();
            switch (response.GetStatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    return JsonSerializer.Deserialize<T>(json);

                default:
                    throw new BadResponseException($"Get {typeof(T)} fail : {response.GetStatusCode()}", response.GetStatusCode(), response);
            }
        }

        public async Task<string> SendTokenRequest()
        {
            IApiResponse response = await _webClient.RequestAccessTokenAsync();
            switch (response.GetStatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    AuthToken credentials = JsonSerializer.Deserialize<AuthToken>(json);
                    _token = credentials.AccessToken;
                    int expiresInSeconds = credentials.ExpiresIn;
                    _tokenExpiration = DateTime.Now.AddSeconds(expiresInSeconds);
                    return _token;

                default:
                    throw new HttpRequestException($"Send Token Error : {response.GetStatusCode()}");
            }
        }

        public async Task<IApiResponse> Authentificate()
        {
            IApiResponse response = await _webClient.RequestUserAuthorizeAsync();
            return (response.GetStatusCode()) switch
            {
                HttpStatusCode.OK => response,
                _ => throw new HttpRequestException($"Login Error : {response.GetStatusCode()}"),
            };
        }

        private void ThrowIfInvalidRequest()
        {
            VerifyConfigurationIsValid();
            if (_config.AnyReachedLimit())
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
            if (_config == null || _webClient == null)
                throw new NullReferenceException("ApiConfiguration is not set, either declare one as global configuration or set a local instance configuration object.");
        }

        private string ParsePath(string query, Namespace? ns = null, string addtionalParams = null)
        {
            string path = ParseSpecialCharacters(query.Trim())
                + "?locale=" + _config.GetLocaleString()
                + "&access_token=" + _token;
            if (ns.HasValue)
                path += $"&namespace={ns.Value.GetCode()}-{_config.GetRegionString().ToLower()}";
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