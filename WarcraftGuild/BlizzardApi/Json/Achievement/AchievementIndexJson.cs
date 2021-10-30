using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("achievements")]
        public List<AchievementJson> Achievements { get; set; }
    }
}