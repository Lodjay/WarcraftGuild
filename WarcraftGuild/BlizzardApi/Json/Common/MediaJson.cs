using System;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class MediaJson
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
   
        public string Type { get; set; }
    }
}
