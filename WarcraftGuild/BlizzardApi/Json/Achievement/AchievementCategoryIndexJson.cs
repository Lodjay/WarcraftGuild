using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class AchievementCategoryIndexJson : WoWJson
    {
        [JsonPropertyName("categories")]
        public List<AchievementCategoryJson> Categories { get; set; }
    }
}