using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Json
{
    public abstract class WoWJson
    {
        public HttpStatusCode? ResultCode { get; set; }
        public bool DirectlyCalled { get; set; }
    }
}