using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestJson : WoWJson
    {
        [JsonPropertyName("emblem")]
        public GuildCrestEmblemJson Emblem { get; set; }

        [JsonPropertyName("border")]
        public GuildCrestBorderJson Border { get; set; }

        [JsonPropertyName("background")]
        public GuildCrestBackgroundJson Background { get; set; }
    }
}