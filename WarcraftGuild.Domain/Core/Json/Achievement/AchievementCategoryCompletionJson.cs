using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class AchievementCategoryCompletionJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("category")]
        public AchievementCategoryJson Category { get; set; }

        [JsonPropertyName("quantity")]
        public uint Quantity { get; set; }

        [JsonPropertyName("points")]
        public uint Points { get; set; }
    }
}