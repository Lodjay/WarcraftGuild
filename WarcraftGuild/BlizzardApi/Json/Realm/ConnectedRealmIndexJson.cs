using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ConnectedRealmIndexJson : WoWJson
    {
        [JsonPropertyName("connected_realms")]
        public List<HrefJson> ConnectedRealms { get; set; }
    }
}
