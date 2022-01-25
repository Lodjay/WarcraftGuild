using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class GuildCrestBackgroundJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}