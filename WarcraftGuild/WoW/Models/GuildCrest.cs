using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using WarcraftGuild.BlizzardApi.WoWJson;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class GuildCrest
    {
        public GuildCrestEmblem Emblem { get; private set; }
        public GuildCrestBorder Border { get; private set; }
        public GuildCrestBackground Background { get; private set; }

        public GuildCrest() { }

        public GuildCrest(GuildCrestJson guildCrestJson)
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

    public class GuildCrestEmblem
    {
        public int BlizzardId { get; private set; }

        public Color Color { get; private set; }

        public GuildCrestEmblem() { }

        public GuildCrestEmblem(GuildCrestEmblemJson guildCrestEmblemJson)
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

    public class GuildCrestBorder
    {
        public long BlizzardId { get; private set; }

        public Color Color { get; private set; }

        public GuildCrestBorder() { }

        public GuildCrestBorder(GuildCrestBorderJson guildCrestBorderJson)
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


    public class GuildCrestBackground
    {
        public Color Color { get; private set; }

        public GuildCrestBackground() { }

        public GuildCrestBackground(GuildCrestBackgroundJson guildCrestBackgroundJson)
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
