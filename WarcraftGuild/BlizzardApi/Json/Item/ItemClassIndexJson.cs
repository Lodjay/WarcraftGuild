using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ItemClassIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("item_classes")]
        public List<ItemClassJson> ItemClasses { get; set; }
    }
}