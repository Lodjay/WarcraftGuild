using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class AchievementJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }
}