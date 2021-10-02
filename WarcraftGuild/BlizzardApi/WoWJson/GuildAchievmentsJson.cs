using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class GuildAchievementsJson
    {
        [JsonPropertyName("total_quantity")]
        public int TotalQuantity { get; set; }

        [JsonPropertyName("total_points")]
        public int TotalPoints { get; set; }

        [JsonPropertyName("achievements")]
        public IEnumerable<AchievmentCompletionJson> Achievements { get; set; }

        [JsonPropertyName("category_progress")]
        public IEnumerable<GuildAchievmentCategoryProgressJson> CategoriesProgress { get; set; }
    }
}