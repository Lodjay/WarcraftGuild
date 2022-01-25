using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class ClassJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gender_name")]
        public GenderTextJson GenderNames { get; set; }

        [JsonPropertyName("power_type")]
        public TypeJson PowerType { get; set; }

        [JsonPropertyName("specializations")]
        public List<SpecializationJson> Specializations { get; set; }

        public MediaJson Media { get; set; }
    }
}