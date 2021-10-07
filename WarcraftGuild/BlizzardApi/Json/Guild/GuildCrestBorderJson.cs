using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestBorderJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}