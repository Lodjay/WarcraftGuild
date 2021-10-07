using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestBackgroundJson : WoWJson
    {
        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}