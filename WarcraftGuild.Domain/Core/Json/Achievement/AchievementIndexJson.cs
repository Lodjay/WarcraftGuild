using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class AchievementIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("achievements")]
        public List<AchievementJson> Achievements { get; set; }
    }
}