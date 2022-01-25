using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class ColorJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("rgba")]
        public ColorCodeApiData ColorCode { get; set; }
    }
}