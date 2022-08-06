using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.WoWHeadApi.Configuration
{
    public class WoWHeadApiConfiguration
    {
        public Locale Locale { get; set; }
        public string RedirectUrl { get; set; }
        private const string API_URL_TEMPLATE = "https://{LOCALE}.wowhead.com/";
        private readonly string apiUrl = string.Empty;

        public WoWHeadApiConfiguration()
        {
            apiUrl = API_URL_TEMPLATE.Replace("{LOCALE}", GetLocaleString().ToLower());
        }


        public string GetLocaleString()
        {
            switch (Locale)
            {
                case Locale.Korean:
                    return "ko";
                case Locale.German:
                    return "de";
                case Locale.French:
                    return "fr";
                case Locale.Italian:
                    return "it";
                case Locale.PortugalPortuguese:
                case Locale.BrazilianPortuguese:
                    return "pt";
                case Locale.MexicanSpanish:
                case Locale.SpainSpanish:
                    return "es";
                case Locale.Russian:
                    return "ru";
                case Locale.SimplifiedChinese:
                case Locale.TraditionalChinese:
                    return "cn";
                case Locale.AmericanEnglish:
                case Locale.BritishEnglish:
                default:
                    return "www";
            }
        }

        public Uri GetApiUrl()
        {
            return new Uri(apiUrl);
        }
    }
}