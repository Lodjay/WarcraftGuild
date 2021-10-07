using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class TypeJson
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}