using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class GuildRosterJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("members")]
        public IEnumerable<GuildMemberJson> Members { get; set; }
    }
}