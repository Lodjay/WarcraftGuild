using System;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class HrefJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("href")]
        public Uri Uri { get; set; }
    }
}