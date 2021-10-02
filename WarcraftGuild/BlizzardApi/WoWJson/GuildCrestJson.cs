using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestJson : BlizzardJson
    {
        [JsonPropertyName("emblem")]
        public GuildCrestEmblemJson Emblem { get; set; }

        [JsonPropertyName("border")]
        public GuildCrestBorderJson Border { get; set; }

        [JsonPropertyName("background")]
        public GuildCrestBackgroundJson Background { get; set; }
    }

    public class GuildCrestEmblemJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }

    public class GuildCrestBorderJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }

    public class GuildCrestBackgroundJson : BlizzardJson
    {
        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}