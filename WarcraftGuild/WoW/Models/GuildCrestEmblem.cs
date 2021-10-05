using System.Drawing;
using WarcraftGuild.BlizzardApi.Json;

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
            Color = Color.FromArgb(
                (int)(guildCrestEmblemJson.Color.ColorCode.A * 255),
                guildCrestEmblemJson.Color.ColorCode.R,
                guildCrestEmblemJson.Color.ColorCode.G,
                guildCrestEmblemJson.Color.ColorCode.B);
        }
    }
}