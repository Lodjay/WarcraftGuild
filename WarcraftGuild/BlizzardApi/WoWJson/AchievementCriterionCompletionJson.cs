using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class AchievementCriterionCompletionJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("is_completed")]
        public bool IsCompleted { get; set; }

        [JsonPropertyName("child_criteria")]
        public IEnumerable<AchievementCriterionCompletionJson> ChildCriteria { get; set; }
    }
}