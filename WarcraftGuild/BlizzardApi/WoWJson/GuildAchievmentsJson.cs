using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class GuildAchievementsJson
    {
        [JsonPropertyName("total_quantity")]
        public uint TotalQuantity { get; set; }

        [JsonPropertyName("total_points")]
        public uint TotalPoints { get; set; }

        [JsonPropertyName("achievements")]
        public IEnumerable<AchievmentCompletionJson> Achievements { get; set; }

        [JsonPropertyName("category_progress")]
        public IEnumerable<AchievementCategoryCompletionJson> CategoriesProgress { get; set; }
    }
}