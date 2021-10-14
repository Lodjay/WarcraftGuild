using System;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class Achievement : WoWModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Points { get; set; }
        public bool AccountWide { get; set; }
        public int Order { get; set; }
        public AchievementCriterion Criterion { get; set; }
        public ulong CategoryId { get; set; }
        public ulong PrerequisiteId { get; set; }
        public Uri Icon { get; set; }

        public Achievement()
        {
        }

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
                if (achievementJson.Criterion != null)
                    Criterion = new AchievementCriterion(achievementJson.Criterion);
                else
                    Criterion = null;
                if (achievementJson.Category != null)
                    CategoryId = achievementJson.Category.Id;
                if (achievementJson.Prerequisite != null)
                    PrerequisiteId = achievementJson.Prerequisite.Id;
                if (achievementJson.Media != null && achievementJson.Media.Assets != null && achievementJson.Media.Assets.Any(x => x.Key == "icon"))
                    Icon = new Uri(achievementJson.Media.Assets.Find(x => x.Key == "icon").Value);
            }
        }
    }
}