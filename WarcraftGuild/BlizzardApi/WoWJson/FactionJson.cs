using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson
{
    public class FactionJson
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}