using System.Drawing;
using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
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
            Color = Color.FromArgb(
                (int)(guildCrestBackgroundJson.Color.ColorCode.A * 255),
                guildCrestBackgroundJson.Color.ColorCode.R,
                guildCrestBackgroundJson.Color.ColorCode.G,
                guildCrestBackgroundJson.Color.ColorCode.B);
        }
    }
}