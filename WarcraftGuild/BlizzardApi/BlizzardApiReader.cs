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

        public async Task<T> GetAsync<T>(string query, string additionalParams = null)
        {
            ThrowIfInvalidRequest();
            if (HasTokenExpired())
                await SendTokenRequest();
            string urlRequest = ParsePath(query, additionalParams);
            IApiResponse response = await _webClient.MakeApiRequestAsync(Configuration.GetApiUrl() + urlRequest);
            Configuration.NotifyAllLimits(this, response);

            if (response.IsSuccessful())
            {
                string json = await response.ReadContentAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                throw new BadResponseException("Response is not successful", response);
            }

        }

        public async Task<string> SendTokenRequest()
        {
            var response = await _webClient.RequestAccessTokenAsync();
            if (response.IsSuccessful())
            {
                string json = await response.ReadContentAsync();
                ClientCredentials credentials = JsonSerializer.Deserialize<ClientCredentials>(json);
                _token = credentials.AccessToken;
                int expiresInSeconds = credentials.ExpiresIn;
                _tokenExpiration = DateTime.Now.AddSeconds(expiresInSeconds);
                return _token;
            }
            if (response.StatusCode() == HttpStatusCode.Unauthorized)
                throw new HttpRequestException("Unauthorized");
            throw new HttpRequestException("response code was not successful");
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

        private string ParsePath(string query, string addtionalParams = null)
        {
            if (addtionalParams is null)
            {
                return ParseSpecialCharacters(query)
                + "?locale=" + Configuration.GetLocaleString()
                + "&access_token=" + _token;
            }
            else
            {
                return ParseSpecialCharacters(query)
                + "?locale=" + Configuration.GetLocaleString()
                + "&access_token=" + _token
                + addtionalParams;
            }

        }
        protected string ParseSpecialCharacters(string s)
        {
            s = s.Replace("#", "%23");
            return s;
        }
    }
}