using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class AchievementCategoryIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("categories")]
        public List<AchievementCategoryJson> Categories { get; set; }
    }
}