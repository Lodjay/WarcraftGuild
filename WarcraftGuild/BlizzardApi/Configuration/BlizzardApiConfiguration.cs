using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;
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
        public List<Limiter> Limiter { get; set; }

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
            return Limiter.Any(i => i.IsAtRateLimit());
        }

        public void NotifyAllLimits()
        {
            foreach (Limiter limit in Limiter)
                limit.OnHttpRequest();
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

        public string GetApiUrl()
        {
            return apiUrl;
        }
    }
}
