using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class GuildCrest : WoWModel
    {
        public GuildCrestEmblem Emblem { get; set; }
        public GuildCrestBorder Border { get; set; }
        public GuildCrestBackground Background { get; set; }

        public GuildCrest()
        {
        }

        public GuildCrest(GuildCrestJson guildCrestJson) : this()
        {
            Load(guildCrestJson);
        }

        public void Load(GuildCrestJson guildCrestJson)
        {
            Emblem = new GuildCrestEmblem(guildCrestJson.Emblem);
            Border = new GuildCrestBorder(guildCrestJson.Border);
            Background = new GuildCrestBackground(guildCrestJson.Background);
        }
    }
}