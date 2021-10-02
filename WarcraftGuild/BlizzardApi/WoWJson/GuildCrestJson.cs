using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson
{
    public class GuildCrestJson
    {
        [JsonPropertyName("emblem")]
        public GuildCrestEmblemJson Emblem { get; set; }

        [JsonPropertyName("border")]
        public GuildCrestBorderJson Border { get; set; }

        [JsonPropertyName("background")]
        public GuildCrestBackgroundJson Background { get; set; }
    }

    public class GuildCrestEmblemJson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }

    public class GuildCrestBorderJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
    public class GuildCrestBackgroundJson
    {
        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}
