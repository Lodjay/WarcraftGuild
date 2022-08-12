using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.WoW.Models.Common;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrestBackground : WoWModel
    {
        public Color Color { get; set; }

        public GuildCrestBackground()
        {
        }

        public GuildCrestBackground(GuildCrestBackgroundJson guildCrestBackgroundJson) : this()
        {
            Load(guildCrestBackgroundJson);
        }

        public void Load(GuildCrestBackgroundJson guildCrestBackgroundJson)
        {
            Color = new Color(guildCrestBackgroundJson.Color);
        }
    }
}