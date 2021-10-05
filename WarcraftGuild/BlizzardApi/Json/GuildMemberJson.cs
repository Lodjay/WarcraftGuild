using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildMemberJson : WoWJson
    {
        [JsonPropertyName("character")]
        public CharacterJson Member { get; set; }

        [JsonPropertyName("rank")]
        public short Rank { get; set; }
    }

}