using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ColorJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("rgba")]
        public ColorCodeApiData ColorCode { get; set; }
    }
}