using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCriterionCompletionJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("amount")]
        public ulong Amount { get; set; }

        [JsonPropertyName("is_completed")]
        public bool IsCompleted { get; set; }

        [JsonPropertyName("child_criteria")]
        public IEnumerable<AchievementCriterionCompletionJson> ChildCriteria { get; set; }
    }
}