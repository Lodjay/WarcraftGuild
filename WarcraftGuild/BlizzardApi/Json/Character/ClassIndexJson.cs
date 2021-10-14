using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarcraftGuild.BlizzardApi.Json
{
    public class ClassIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("classes")]
        public List<ClassJson> Classes { get; set; }
    }
}