using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class RaceIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("races")]
        public List<RaceJson> Races { get; set; }
    }
}