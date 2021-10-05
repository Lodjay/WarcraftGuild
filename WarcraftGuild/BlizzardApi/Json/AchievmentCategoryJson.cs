using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class AchievmentCategoryJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }
}