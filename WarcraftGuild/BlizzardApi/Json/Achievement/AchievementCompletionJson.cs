using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementCompletionJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("criteria")]
        public AchievementCriterionCompletionJson Criteria { get; set; }

        [JsonPropertyName("completed_timestamp")]
        public double? CompletedTimestamp { get; set; }
    }
}