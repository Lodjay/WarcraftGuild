using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class RaceJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}