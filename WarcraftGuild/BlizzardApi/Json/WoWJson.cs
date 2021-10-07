using System.Net;

namespace WarcraftGuild.BlizzardApi.Json
{
    public abstract class WoWJson
    {
        public HttpStatusCode? ResultCode { get; set; }
        public bool DirectlyCalled { get; set; }
    }
}