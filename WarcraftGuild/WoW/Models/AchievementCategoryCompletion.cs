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
    public class AchievementCategoryCompletion : WoWData
    {
        public uint Quantity { get; set; }
        public uint Points { get; set; }

        public AchievementCategoryCompletion() { }

        public AchievementCategoryCompletion(AchievementCategoryCompletionJson achievementCategoryCompletionJson) : this()
        {
            Load(achievementCategoryCompletionJson);
        }

        public void Load(AchievementCategoryCompletionJson achievementCategoryCompletionJson)
        {
            if (CanLoadJson(achievementCategoryCompletionJson))
            {
                BlizzardId = achievementCategoryCompletionJson.Category.Id;
                Quantity = achievementCategoryCompletionJson.Quantity;
                Points = achievementCategoryCompletionJson.Points;
            }
        }
    }
}
