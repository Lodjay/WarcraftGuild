using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementIndexJson : WoWJson
    {
        [JsonPropertyName("achievements")]
        public List<AchievementJson> Achievements { get; set; }
    }
}