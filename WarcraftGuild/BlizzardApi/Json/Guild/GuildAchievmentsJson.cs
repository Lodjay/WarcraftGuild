using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildAchievementsJson : WoWJson
    {
        [JsonPropertyName("total_quantity")]
        public uint TotalQuantity { get; set; }

        [JsonPropertyName("total_points")]
        public uint TotalPoints { get; set; }

        [JsonPropertyName("achievements")]
        public IEnumerable<AchievementCompletionJson> Achievements { get; set; }

        [JsonPropertyName("category_progress")]
        public IEnumerable<AchievementCategoryCompletionJson> CategoriesProgress { get; set; }
    }
}