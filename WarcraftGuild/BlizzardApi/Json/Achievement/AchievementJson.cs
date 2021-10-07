using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("points")]
        public uint Points { get; set; }

        [JsonPropertyName("is_account_wide")]
        public bool AccountWide { get; set; }

        [JsonPropertyName("display_order")]
        public int Order { get; set; }

        [JsonPropertyName("category")]
        public AchievementCategoryJson Category { get; set; }

        [JsonPropertyName("prerequisite_achievement")]
        public AchievementJson Prerequisite { get; set; }

        [JsonPropertyName("reward_description")]
        public string RewardDescription { get; set; }

        [JsonPropertyName("reward_item")]
        public ItemJson RewardItem { get; set; }

        public MediaJson Media { get; set; }
    }
}