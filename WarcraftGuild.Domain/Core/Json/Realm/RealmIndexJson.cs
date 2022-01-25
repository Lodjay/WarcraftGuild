using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class RealmIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("realms")]
        public List<RealmJson> Realms { get; set; }
    }
}