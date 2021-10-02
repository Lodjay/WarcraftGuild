using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class AchievementCategoryCompletionJson
    {
        [JsonPropertyName("category")]
        public AchievmentCategoryIdJson Category { get; set; }

        [JsonPropertyName("quantity")]
        public uint Quantity { get; set; }

        [JsonPropertyName("points")]
        public uint Points { get; set; }
    }

    public class AchievmentCategoryIdJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }
}