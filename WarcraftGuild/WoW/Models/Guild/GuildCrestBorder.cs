using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.WoW.Models.Common;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrestBorder : WoWModel
    {
        public string ImageUrl { get; set; }

        public Color Color { get; set; }

        public GuildCrestBorder()
        {
        }

        public GuildCrestBorder(GuildCrestBorderJson guildCrestBorderJson) : this()
        {
            Load(guildCrestBorderJson);
        }

        public void Load(GuildCrestBorderJson guildCrestBorderJson)
        {
            BlizzardId = guildCrestBorderJson.Id;
            Color = new Color(guildCrestBorderJson.Color);
        }
    }
}