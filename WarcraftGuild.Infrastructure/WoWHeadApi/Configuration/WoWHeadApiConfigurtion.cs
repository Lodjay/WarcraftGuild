using WarcraftGuild.Domain.Core.Enums;

namespace WarcraftGuild.Infrastructure.WoWHeadApi.Configuration
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
            return Locale switch
            {
                Locale.Korean => "ko",
                Locale.German => "de",
                Locale.French => "fr",
                Locale.Italian => "it",
                Locale.PortugalPortuguese or Locale.BrazilianPortuguese => "pt",
                Locale.MexicanSpanish or Locale.SpainSpanish => "es",
                Locale.Russian => "ru",
                Locale.SimplifiedChinese or Locale.TraditionalChinese => "cn",
                _ => "www",
            };
        }

        public Uri GetApiUrl()
        {
            return new Uri(apiUrl);
        }
    }
}