using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json

{
    public class GuildJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("crest")]
        public GuildCrestJson Crest { get; set; }

        [JsonPropertyName("faction")]
        public TypeJson Faction { get; set; }

        [JsonPropertyName("created_timestamp")]
        public double CreationTimestamp { get; set; }
    }
}