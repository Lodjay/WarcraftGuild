using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

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