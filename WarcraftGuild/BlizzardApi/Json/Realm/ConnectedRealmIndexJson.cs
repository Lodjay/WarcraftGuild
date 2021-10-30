using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ConnectedRealmIndexJson : BlizzardApiJsonResponse
    {

        [JsonPropertyName("connected_realms")]
        public List<HrefJson> ConnectedRealms { get; set; }
    }
}