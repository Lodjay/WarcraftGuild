using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class MediaJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("assets")]
        public List<AssetJson> Assets { get; set; }

        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        // Old type? 
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonPropertyName("bust_url")]
        public string InsetUrl { get; set; }
        [JsonPropertyName("render_url")]
        public string RenderUrl { get; set; }
    }
}