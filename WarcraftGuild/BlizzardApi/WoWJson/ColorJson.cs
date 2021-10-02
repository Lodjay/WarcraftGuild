using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ColorJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("rgba")]
        public ColorCodeApiData ColorCode { get; set; }
    }

    public class ColorCodeApiData : BlizzardJson
    {
        [JsonPropertyName("r")]
        public byte R { get; set; }

        [JsonPropertyName("g")]
        public byte G { get; set; }

        [JsonPropertyName("b")]
        public byte B { get; set; }

        [JsonPropertyName("a")]
        public float A { get; set; }
    }
}