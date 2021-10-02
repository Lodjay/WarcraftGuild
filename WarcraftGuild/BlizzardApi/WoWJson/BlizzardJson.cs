using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class BlizzardJson
    {
        public HttpStatusCode? ResultCode { get; set; }
    }
}