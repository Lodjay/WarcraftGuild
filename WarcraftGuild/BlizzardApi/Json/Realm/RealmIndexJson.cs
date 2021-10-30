using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class RealmIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("realms")]
        public List<RealmJson> Realms { get; set; }
    }
}