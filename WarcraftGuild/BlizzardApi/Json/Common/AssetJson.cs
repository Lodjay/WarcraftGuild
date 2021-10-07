using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class AssetJson : WoWJson
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("file_data_id")]
        public ulong Id { get; set; }
    }
}