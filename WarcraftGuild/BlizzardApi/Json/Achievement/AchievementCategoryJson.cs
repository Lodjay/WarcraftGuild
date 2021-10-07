using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class AchievementCategoryJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_guild_category")]
        public bool GuildCategory { get; set; }

        [JsonPropertyName("display_order")]
        public int Order { get; set; }

        [JsonPropertyName("parent_category")]
        public AchievementCategoryJson ParentCategory { get; set; }

        [JsonPropertyName("achievements")]
        public List<AchievementJson> Achievments { get; set; }
    }
}