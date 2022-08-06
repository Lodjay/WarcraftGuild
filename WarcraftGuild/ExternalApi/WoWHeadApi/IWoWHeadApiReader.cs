using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WarcraftGuild.WoWHeadApi.Xml;

namespace WarcraftGuild.WoWHeadApi
{
    public interface IWoWHeadApiReader
    {
        TXml GetXmlAsync<TXml>(string query, string additionalParams = null) where TXml : WoWHeadApiXmlResponse, new();
    }
}
