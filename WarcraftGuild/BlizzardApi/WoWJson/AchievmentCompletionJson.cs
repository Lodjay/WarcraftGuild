using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class AchievmentCompletionJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("criteria")]
        public AchievementCriterionCompletionJson Criteria { get; set; }

        [JsonPropertyName("completed_timestamp")]
        public double? CompletedTimestamp { get; set; }
    }
}