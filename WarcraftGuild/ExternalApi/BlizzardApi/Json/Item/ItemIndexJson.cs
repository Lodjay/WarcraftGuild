using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ItemIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}