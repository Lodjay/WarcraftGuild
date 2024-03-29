﻿using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Json;
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
            _config = apiConfiguration?.Value ?? throw new ArgumentNullException(nameof(apiConfiguration));
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        public async Task<string> GetJsonAsync(string query, Namespace? ns = null, bool force = false)
        {
            await Check(force).ConfigureAwait(false);
            string urlRequest = ParsePath(query, ns);
            IApiResponse response = await _webClient.MakeApiRequestAsync(new Uri(_config.GetApiUrl(), urlRequest).AbsoluteUri);
            
            switch (response.GetStatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    return json;

                default:
                    throw new BadResponseException($"Get JSON fail : {response.GetStatusCode()}", response.GetStatusCode(), response);
            }
        }

        public async Task<WoWJson> GetAsync<WoWJson>(string query, Namespace? ns = null, bool force = false) where WoWJson : BlizzardApiJsonResponse, new()
        {
            await Check(force).ConfigureAwait(false);
            string urlRequest = ParsePath(query, ns);
            IApiResponse response = await _webClient.MakeApiRequestAsync(new Uri(_config.GetApiUrl(), urlRequest).AbsoluteUri);
            switch (response.GetStatusCode())
            {
                case HttpStatusCode.OK:
                    string json = await response.ReadContentAsync();
                    WoWJson result = JsonSerializer.Deserialize<WoWJson>(json);
                    result.ResultCode = response.GetStatusCode();
                    result.DirectlyCalled = true;
                    return result;

                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                    return new WoWJson() { ResultCode = response.GetStatusCode() };

                default:
                    throw new BadResponseException($"Get {typeof(WoWJson)} fail : {response.GetStatusCode()}", response.GetStatusCode(), response);
            }
        }

        public async Task Check(bool force = false)
        {
            ThrowIfInvalidRequest(force);
            if (HasTokenExpired())
                await SendTokenRequest();
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

        public Limiter GetShorterLimiter()
        {
            return _config.GetShorterLimiter();
        }

        private void CheckTimeLimit(bool force = false)
        {
            if (_config.AnyReachedLimit())
            {
                if (force)
                {
                    Task.Delay(GetShorterLimiter().TimeBetweenLimitReset);
                    CheckTimeLimit(force);
                }
                else
                    throw new RateLimitReachedException("http request was blocked by RateLimiter");
            }
            else
                _config.NotifyAllLimits();
        }

        private void ThrowIfInvalidRequest(bool force = false)
        {
            CheckTimeLimit(force);
        }

        private bool HasTokenExpired()
        {
            if (string.IsNullOrEmpty(_token)
                || DateTime.Now > _tokenExpiration)
                return true;
            return false;
        }

        private string ParsePath(string query, Namespace? ns = null)
        {
            string path = ParseSpecialCharacters(query.Trim())
                + "?locale=" + _config.GetLocaleString()
                + "&access_token=" + _token;
            if (ns.HasValue)
                path += $"&namespace={ns.Value.GetCode()}-{_config.GetRegionString().ToLower()}";
            return path;
        }

        private static string ParseSpecialCharacters(string s)
        {
            return s.Replace("#", "%23");
        }
    }
}