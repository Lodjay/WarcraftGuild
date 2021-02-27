using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.WoWJson

{
    public class GuildAchievmentCategoryProgressJson
    {
        [JsonPropertyName("category")]
        public long Category { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }
    }

    public class GuildAchievmentCategoryJson
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
