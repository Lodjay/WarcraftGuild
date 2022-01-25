using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class ItemClassIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("item_classes")]
        public List<ItemClassJson> ItemClasses { get; set; }
    }
}