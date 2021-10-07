using System.Collections.Generic;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ConnectedRealmJson : WoWJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("has_queue")]
        public bool HasQueue { get; set; }

        [JsonPropertyName("status")]
        public TypeJson Status { get; set; }

        [JsonPropertyName("population")]
        public TypeJson Population { get; set; }

        [JsonPropertyName("realms")]
        public List<RealmJson> Realms { get; set; }
    }
}