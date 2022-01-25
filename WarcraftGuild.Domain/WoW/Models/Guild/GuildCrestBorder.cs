using System.Drawing;
using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
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
            Color = Color.FromArgb(
                (int)(guildCrestBorderJson.Color.ColorCode.A * 255),
                guildCrestBorderJson.Color.ColorCode.R,
                guildCrestBorderJson.Color.ColorCode.G,
                guildCrestBorderJson.Color.ColorCode.B);
        }
    }
}