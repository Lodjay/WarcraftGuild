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
<<<<<<< HEAD
        public IEnumerable<GuildAchievmentCategoryProgressJson> CategoriesProgress { get; set; }
=======
        public IEnumerable<AchievementCategoryCompletionJson> CategoriesProgress { get; set; }

>>>>>>> f12ccfa... Get Guild + Achievements
    }
}