using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class CharacterJson : WoWJson
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

    }
}