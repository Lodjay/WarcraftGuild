using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class ConnectedRealmIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("connected_realms")]
        public List<HrefJson> ConnectedRealms { get; set; }
    }
}