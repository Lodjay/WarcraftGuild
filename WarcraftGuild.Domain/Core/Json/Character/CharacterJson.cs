using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class CharacterJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender")]
        public TypeJson Gender { get; set; }

        [JsonPropertyName("faction")]
        public TypeJson Faction { get; set; }

        [JsonPropertyName("race")]
        public RaceJson Race { get; set; }

        [JsonPropertyName("character_class")]
        public ClassJson Class { get; set; }

        [JsonPropertyName("level")]
        public ushort Level { get; set; }

        [JsonPropertyName("realm")]
        public RealmJson Realm { get; set; }

        public MediaJson Media { get; set; }
    }
}