using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.BlizzardApi.Configuration
{
    public class BlizzardApiConfiguration
    {
        public Region ApiRegion { get; set; }
        public Locale Locale { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public List<Limiter> Limiters { get; set; }

        private const string AUTH_URL_TEMPLATE = "https://{REGION}.battle.net/oauth/";
        private const string API_URL_TEMPLATE = "https://{REGION}.api.blizzard.com/";
        private readonly string authUrl = string.Empty;
        private readonly string apiUrl = string.Empty;

        public BlizzardApiConfiguration()
        {
            apiUrl = API_URL_TEMPLATE.Replace("{REGION}", GetRegionString().ToLower());
            authUrl = AUTH_URL_TEMPLATE.Replace("{REGION}", GetRegionString().ToLower());
        }

        public bool AnyReachedLimit()
        {
            return Limiters.Any(i => i.IsAtRateLimit());
        }

        public void NotifyAllLimits()
        {
            foreach (Limiter limit in Limiters)
                limit.OnHttpRequest();
        }

        public Limiter GetShorterLimiter()
        {
            Limiter result = null;
            TimeSpan tmp = Limiters.FirstOrDefault().TimeBetweenLimitReset;
            foreach (Limiter limiter in Limiters)
            {
                if (tmp >= limiter.TimeBetweenLimitReset)
                {
                    result = limiter.Clone();
                    tmp = result.TimeBetweenLimitReset;
                }
            }
            return result;
        }

        public string GetLocaleString()
        {
            return Locale.GetCode();
        }

        public string GetRegionString()
        {
            return ApiRegion.GetCode();
        }

        public string GetAuthUrl()
        {
            return authUrl;
        }

        public Uri GetApiUrl()
        {
            return new Uri(apiUrl);
        }
    }
}