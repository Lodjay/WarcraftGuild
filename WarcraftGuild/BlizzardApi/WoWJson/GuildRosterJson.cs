using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class GuildRosterJson
    {
        [JsonPropertyName("members")]
        public IEnumerable<GuildMemberJson> Members { get; set; }
    }

    public class GuildMemberJson
    {
        [JsonPropertyName("character")]
        public MemberJson Member { get; set; }

        [JsonPropertyName("rank")]
        public short Rank { get; set; }
    }

    public class MemberJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
    }

}