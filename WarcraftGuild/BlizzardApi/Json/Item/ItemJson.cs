using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ItemJson : WoWJson
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}