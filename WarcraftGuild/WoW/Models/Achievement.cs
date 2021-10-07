using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Achievement : WoWModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Points { get; set; }
        public bool AccountWide { get; set; }
        public int Order { get; set; }
        public ulong CategoryId { get; set; }
        public ulong PrerequisiteId { get; set; }

        public Achievement() { }

        public Achievement(AchievementJson achievementJson) : this()
        {
            Load(achievementJson);
        }

        public void Load(AchievementJson achievementJson)
        {
            if (CheckJson(achievementJson))
            {
                BlizzardId = achievementJson.Id;
                Name = achievementJson.Name;
                Description = achievementJson.Description;
                Points = achievementJson.Points;
                AccountWide = achievementJson.AccountWide;
                Order = achievementJson.Order;
                if (achievementJson.Category != null)
                    CategoryId = achievementJson.Category.Id;
                if (achievementJson.Prerequisite != null)
                    PrerequisiteId = achievementJson.Prerequisite.Id;
            }
        }
    }
}
