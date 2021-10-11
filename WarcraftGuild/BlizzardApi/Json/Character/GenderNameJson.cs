using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GenderNameJson
    {
        [JsonPropertyName("male")]
        public string Male { get; set; }
        [JsonPropertyName("female")]
        public string Female { get; set; }
    }
}