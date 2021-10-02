﻿using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json

{
    public class GuildJson : BlizzardJson
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