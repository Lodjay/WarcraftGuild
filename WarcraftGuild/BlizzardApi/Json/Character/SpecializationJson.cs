using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class SpecializationJson : WoWJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("playable_class")]
        public ClassJson Class { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender_description")]
        public GenderTextJson GenderDescriptions { get; set; }

        [JsonPropertyName("role")]
        public TypeJson Role { get; set; }

        public MediaJson Media { get; set; }
    }
}