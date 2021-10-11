using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class RaceIndexJson : WoWJson
    {
        [JsonPropertyName("races")]
        public List<RaceJson> Races { get; set; }
    }
}