using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class RealmJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("is_tournament")]
        public bool Tournament { get; set; }

        [JsonPropertyName("type")]
        public TypeJson Type { get; set; }
    }
}