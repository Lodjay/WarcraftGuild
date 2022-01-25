using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class HrefJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("href")]
        public Uri Uri { get; set; }
    }
}