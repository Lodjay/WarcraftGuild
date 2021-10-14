using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildRosterJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("members")]
        public IEnumerable<GuildMemberJson> Members { get; set; }
    }
}