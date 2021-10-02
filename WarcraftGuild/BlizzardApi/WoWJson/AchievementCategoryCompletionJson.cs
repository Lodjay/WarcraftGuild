using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCategoryCompletionJson : BlizzardJson
    {
        [JsonPropertyName("category")]
        public AchievmentCategoryIdJson Category { get; set; }

        [JsonPropertyName("quantity")]
        public uint Quantity { get; set; }

        [JsonPropertyName("points")]
        public uint Points { get; set; }
    }

    public class AchievmentCategoryIdJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }
}