using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class MediaJson : WoWJson
    {
        [JsonPropertyName("assets")]
        public List<AssetJson> Assets { get; set; }

        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }
}