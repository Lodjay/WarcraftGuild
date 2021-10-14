using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class RaceJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender_name")]
        public GenderTextJson GenderNames { get; set; }

        [JsonPropertyName("faction")]
        public TypeJson Faction { get; set; }

        [JsonPropertyName("is_selectable")]
        public bool Selectable { get; set; }

        [JsonPropertyName("is_allied_race")]
        public bool AlliedRace { get; set; }
    }
}