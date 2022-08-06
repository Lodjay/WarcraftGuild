using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class GuildCrestBackgroundJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("color")]
        public ColorJson Color { get; set; }
    }
}