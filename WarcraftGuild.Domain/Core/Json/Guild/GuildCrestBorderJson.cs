using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class GuildCrestBorderJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}