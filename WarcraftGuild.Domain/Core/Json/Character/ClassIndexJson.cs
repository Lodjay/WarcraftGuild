using System.Text.Json.Serialization;

namespace WarcraftGuild.Domain.Core.Json
{
    public class ClassIndexJson : BlizzardApiJsonResponse
    {
        [JsonPropertyName("classes")]
        public List<ClassJson> Classes { get; set; }
    }
}