using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestEmblemJson : WoWJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}