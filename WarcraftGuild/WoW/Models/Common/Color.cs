using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.WoW.Models.Common
{
    public class Color
    {
        public byte A { get; set; }
        public byte R { get; set; } 
        public byte G { get; set; }
        public byte B { get; set; }
        public string Code { get; set; }

        public Color()      
        {
        }


        public Color(System.Drawing.Color color) : this()
        {
            Load(color);
        }

        public Color(ColorJson colorJson) : this(
            System.Drawing.Color.FromArgb(
                (int)(colorJson.ColorCode.A * 255),
                colorJson.ColorCode.R,
                colorJson.ColorCode.G,
                colorJson.ColorCode.B))
        {
        }

        public void Load(System.Drawing.Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
            Code = color.Name;
        }
    }
}