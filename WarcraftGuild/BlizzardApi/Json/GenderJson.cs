using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GenderJson
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}