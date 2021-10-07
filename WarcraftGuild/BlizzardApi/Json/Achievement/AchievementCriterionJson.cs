using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCriterionJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("show_progress_bar")]
        public bool ProgressBar { get; set; }

        [JsonPropertyName("operator")]
        public TypeJson Operator { get; set; }

        [JsonPropertyName("child_criteria")]
        public List<AchievementCriterionJson> SubCriteria { get; set; }

    }
}