using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.Enums;

namespace WarcraftGuild.BlizzardApi.Configuration
{
    public class BlizzardApiConfiguration
    {
        public Region ApiRegion { get; set; }
        public Locale Locale { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        private const string AUTH_URL_TEMPLATE = "https://REGION.battle.net/oauth/token";
        private const string API_URL_TEMPLATE = "https://REGION.api.blizzard.com";
        private string authUrl = string.Empty;
        private string apiUrl = string.Empty;

        public BlizzardApiConfiguration()
        {
            SetRegion(Region.Europe, true);
            Locale = ApiRegion.GetDefaultLocale();
        }

        public static BlizzardApiConfiguration Create()
        {
            return new BlizzardApiConfiguration();
        }

        public BlizzardApiConfiguration SetClientId(string clientId)
        {
            ClientId = clientId;
            return this;
        }


        public BlizzardApiConfiguration SetClientSecret(string clientSecret)
        {
            ClientSecret = clientSecret;
            return this;
        }

        public BlizzardApiConfiguration SetRegion(Region region)
        {
            return SetRegion(region, false);
        }

        /// <summary>
        /// Set the region of the ApiConfiguration with locale set to default locale of region if bool is set to true
        /// </summary>
        /// <param name="region">The region to set</param>
        /// <param name="useDefaultLocale">Determines whether locale should be set based on default locale of region</param>
        /// <returns>This instance of ApiConfiguration</returns>
        public BlizzardApiConfiguration SetRegion(Region region, bool useDefaultLocale)
        {
            ApiRegion = region;
            apiUrl = API_URL_TEMPLATE.Replace("REGION", GetRegionString());
            authUrl = AUTH_URL_TEMPLATE.Replace("REGION", GetRegionString());
            if (useDefaultLocale)
            {
                Locale = region.GetDefaultLocale();
            }
            return this;
        }

        public BlizzardApiConfiguration SetLocale(Locale locale)
        {
            Locale = locale;
            return this;
        }

        /// <summary>
        /// Declare this Configuration as the global default configuration, it will be used when no configuration is provided to the api reader.
        /// </summary>  
        public BlizzardApiConfiguration DeclareAsDefault()
        {
            BlizzardApiReader.SetDefaultConfiguration(this);
            return this;
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
