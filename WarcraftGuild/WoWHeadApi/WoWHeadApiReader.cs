using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using WarcraftGuild.WoWHeadApi.Xml;
using WarcraftGuild.WoWHeadApi.Configuration;

namespace WarcraftGuild.WoWHeadApi
{
    public class WoWHeadApiReader : IWoWHeadApiReader
    {
        private readonly WoWHeadApiConfiguration _config;

        public WoWHeadApiReader(IOptions<WoWHeadApiConfiguration> apiConfiguration)
        {
            _config = apiConfiguration.Value;
        }

        public TXml GetXmlAsync<TXml>(string query, string additionalParams = null) where TXml : WoWHeadApiXmlResponse, new()
        {
            string urlRequest = ParsePath(query, additionalParams);
            TXml result = new TXml
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
