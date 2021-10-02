using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class GuildJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("crest")]
        public GuildCrestJson Crest { get; set; }

        [JsonPropertyName("faction")]
        public FactionJson Faction { get; set; }

        [JsonPropertyName("created_timestamp")]
        public double CreationTimestamp { get; set; }
    }
}