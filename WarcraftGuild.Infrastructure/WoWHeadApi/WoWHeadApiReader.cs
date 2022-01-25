using Microsoft.Extensions.Options;
using System.Xml.Linq;
using WarcraftGuild.Domain.Interfaces;
using WarcraftGuild.Domain.Interfaces.Infrastructure;
using WarcraftGuild.Infrastructure.WoWHeadApi.Configuration;

namespace WarcraftGuild.Infrastructure.WoWHeadApi
{
    public class WoWHeadApiReader : IWoWHeadApiReader
    {
        private readonly WoWHeadApiConfiguration _config;

        public WoWHeadApiReader(IOptions<WoWHeadApiConfiguration> apiConfiguration)
        {
            _config = apiConfiguration.Value;
        }

        public TXml GetXmlAsync<TXml>(string query, string additionalParams = "") where TXml : IWoWHeadApiXmlResponse, new()
        {
            string urlRequest = ParsePath(query, additionalParams);
            TXml result = new()
            {
                Xml = XDocument.Load(new Uri(_config.GetApiUrl(), $"{urlRequest}&xml").ToString())
            };
            return result;
        }

        private static string ParsePath(string query, string addtionalParams = null)
        {
            string path = ParseSpecialCharacters(query.Trim());
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