using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson
{
    public class ColorJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("rgba")]
        public ColorCodeApiData ColorCode { get; set; }
    }

    public class ColorCodeApiData
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