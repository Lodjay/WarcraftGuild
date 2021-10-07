using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildRosterJson : WoWJson
    {
        [JsonPropertyName("members")]
        public IEnumerable<GuildMemberJson> Members { get; set; }
    }
}