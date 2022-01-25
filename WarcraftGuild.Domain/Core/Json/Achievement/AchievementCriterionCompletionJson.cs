using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class AchievementCriterionCompletionJson : BlizzardApiJsonResponse
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