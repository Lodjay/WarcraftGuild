using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildRosterJson : BlizzardJson
    {
        [JsonPropertyName("members")]
        public IEnumerable<GuildMemberJson> Members { get; set; }
    }

    public class GuildMemberJson : BlizzardJson
    {
        [JsonPropertyName("character")]
        public MemberJson Member { get; set; }

        [JsonPropertyName("rank")]
        public short Rank { get; set; }
    }

    public class MemberJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        public CharacterJson Character { get; set; }
    }

}