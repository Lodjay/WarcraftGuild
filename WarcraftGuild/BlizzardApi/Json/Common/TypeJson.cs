﻿using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class TypeJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}