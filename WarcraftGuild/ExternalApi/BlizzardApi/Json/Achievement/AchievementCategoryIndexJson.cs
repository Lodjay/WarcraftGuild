using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class AchievementCategoryIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("categories")]
        public List<AchievementCategoryJson> Categories { get; set; }
    }
}