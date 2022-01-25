using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class ItemClassJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("item_subclasses")]
        public List<ItemClassJson> SubClasses { get; set; }
    }
}