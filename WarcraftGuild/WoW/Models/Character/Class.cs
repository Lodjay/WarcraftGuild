﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Class : WoWModel
    {
        public string Name { get; set; }
        public string MaleName { get; set; }
        public string FemaleName { get; set; }
        public string PowerType { get; set; }
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public List<ulong> SpecializationList { get; set; }
        public Uri Icon { get; set; }

        public Class()
        {
            SpecializationList = new List<ulong>();
        }

        public Class(ClassJson classJson) : this()
        {
            Load(classJson);
        }

        public void Load(ClassJson classJson)
        {
            if (CheckJson(classJson))
            {
                BlizzardId = classJson.Id;
                Name = classJson.Name;
                if (classJson.GenderNames != null)
                {
                    MaleName = classJson.GenderNames.Male;
                    FemaleName = classJson.GenderNames.Female;
                }
                if (classJson.PowerType != null)
                    PowerType = classJson.PowerType.Name;
                if (classJson.Specializations != null)
                    foreach (SpecializationJson specialization in classJson.Specializations)
                        SpecializationList.Add(specialization.Id);
                if (classJson.Media != null && classJson.Media.Assets != null && classJson.Media.Assets.Any(x => x.Key == "icon"))
                    Icon = new Uri(classJson.Media.Assets.Find(x => x.Key == "icon").Value);
            }
        }
    }
}