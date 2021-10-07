using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCategoryCompletionJson : WoWJson
    {
        [JsonPropertyName("category")]
        public AchievementCategoryJson Category { get; set; }

        [JsonPropertyName("quantity")]
        public uint Quantity { get; set; }

        [JsonPropertyName("points")]
        public uint Points { get; set; }
    }
}