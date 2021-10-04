using System.Drawing;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrestBorder : WoWData
    {

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
            Color = Color.FromArgb(
                (int)(guildCrestBorderJson.Color.ColorCode.A * 255),
                guildCrestBorderJson.Color.ColorCode.R,
                guildCrestBorderJson.Color.ColorCode.G,
                guildCrestBorderJson.Color.ColorCode.B);
        }
    }
}