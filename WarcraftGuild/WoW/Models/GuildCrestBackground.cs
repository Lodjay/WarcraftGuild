using System.Drawing;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrestBackground : WoWData
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