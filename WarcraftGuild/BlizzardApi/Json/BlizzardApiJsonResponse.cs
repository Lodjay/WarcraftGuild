using System.Net;

namespace WarcraftGuild.BlizzardApi.Json
{
    public abstract class BlizzardApiJsonResponse
    {
        public HttpStatusCode? ResultCode { get; set; }
        public bool DirectlyCalled { get; set; }

        public WoWJson GetDerived<WoWJson>() where WoWJson : BlizzardApiJsonResponse
        {
            return (WoWJson)this;
        }
    }
}