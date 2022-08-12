using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCategoryCompletionJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("category")]
        public AchievementCategoryJson Category { get; set; }

        [JsonPropertyName("quantity")]
        public long Quantity { get; set; }

        [JsonPropertyName("points")]
        public long Points { get; set; }
    }
}