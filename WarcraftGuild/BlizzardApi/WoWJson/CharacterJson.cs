using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class CharacterJson : BlizzardJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender")]
        public GenderJson Gender { get; set; }

        [JsonPropertyName("faction")]
        public FactionJson Faction { get; set; }

        [JsonPropertyName("race")]
        public RaceJson Race { get; set; }

        [JsonPropertyName("character_class")]
        public ClassJson Class { get; set; }

        [JsonPropertyName("level")]
        public ushort Level { get; set; }
    }

    public class GenderJson
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class RaceJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ClassJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}