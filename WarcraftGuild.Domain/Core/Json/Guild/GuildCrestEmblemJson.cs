using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class GuildCrestEmblemJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}