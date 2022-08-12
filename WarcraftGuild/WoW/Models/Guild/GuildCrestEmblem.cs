using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.WoW.Models.Common;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrestEmblem : WoWModel
    {
        public string ImageUrl { get; set; }

        public Color Color { get; set; }

        public GuildCrestEmblem()
        {
        }

        public GuildCrestEmblem(GuildCrestEmblemJson guildCrestEmblemJson) : this()
        {
            Load(guildCrestEmblemJson);
        }

        public void Load(GuildCrestEmblemJson guildCrestEmblemJson)
        {
            BlizzardId = guildCrestEmblemJson.Id;
            Color = new Color(guildCrestEmblemJson.Color);
        }
    }
}