using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCriterion
    {
        public ulong BlizzardId { get; set; }
        public string Description { get; set; }
        public uint Amount { get; set; }
        public bool ProgressBar { get; set; }
        public ulong AchievementId { get; set; }
        public List<AchievementCriterion> SubCriteria { get; set; }


        public AchievementCriterion()
        {
            SubCriteria = new List<AchievementCriterion>();
        }

        public AchievementCriterion(AchievementCriterionJson achievementCriterionJson) : this()
        {
            Load(achievementCriterionJson);
        }

        public void Load(AchievementCriterionJson achievementCriterionJson)
        {
            if (achievementCriterionJson != null)
            {
                BlizzardId = achievementCriterionJson.Id;
                Description = achievementCriterionJson.Description;
                Amount = achievementCriterionJson.Amount;
                ProgressBar = achievementCriterionJson.ProgressBar;
                if (achievementCriterionJson.Achievement != null)
                    AchievementId = achievementCriterionJson.Achievement.Id;
                if (achievementCriterionJson.SubCriteria != null)
                    foreach (AchievementCriterionJson subCriterion in achievementCriterionJson.SubCriteria)
                        SubCriteria.Add(new AchievementCriterion(subCriterion));
            }
        }
    }
}