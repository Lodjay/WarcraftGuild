using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievmentCompletionJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("criteria")]
        public AchievementCriterionCompletionJson Criteria { get; set; }

        [JsonPropertyName("completed_timestamp")]
        public double? CompletedTimestamp { get; set; }
    }
}