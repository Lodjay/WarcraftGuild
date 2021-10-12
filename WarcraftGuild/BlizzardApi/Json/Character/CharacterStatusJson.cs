using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class CharacterStatusJson : WoWJson
    {

        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
    }
}