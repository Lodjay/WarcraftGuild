using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class CharacterJson
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
        public short Level { get; set; }
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
        [JsonPropertyName("type")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ClassJson
    {
        [JsonPropertyName("type")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}