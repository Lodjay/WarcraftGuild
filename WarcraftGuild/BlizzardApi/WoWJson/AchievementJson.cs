using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class AchievementJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}