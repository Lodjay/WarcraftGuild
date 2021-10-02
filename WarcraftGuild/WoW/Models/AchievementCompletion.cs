﻿using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.WoWJson;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCompletion
    {
        public ulong BlizzardId { get; private set; }
        public DateTime? CompletionDate { get; private set; }
        public AchievementCriterionCompletion Criteria { get; private set; }

        public AchievementCompletion()
        {
        }

        public AchievementCompletion(AchievmentCompletionJson achievmentCompletionJson) : base()
        {
            Load(achievmentCompletionJson);
        }

        public void Load(AchievmentCompletionJson achievmentCompletionJson)
        {
            BlizzardId = achievmentCompletionJson.Id;
            if (achievmentCompletionJson.CompletedTimestamp.HasValue)
                CompletionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(achievmentCompletionJson.CompletedTimestamp.Value);
            else
                CompletionDate = null;
            if (achievmentCompletionJson.Criteria != null)
                Criteria = new AchievementCriterionCompletion(achievmentCompletionJson.Criteria);
        }
    }

    public class AchievementCriterionCompletion
    {
        public ulong BlizzardId { get; private set; }
        public ulong Amount { get; private set; }
        public bool IsCompleted { get; private set; }
        public List<AchievementCriterionCompletion> ChildCriteria { get; set; }

        public AchievementCriterionCompletion()
        {
            ChildCriteria = new List<AchievementCriterionCompletion>();
        }

        public AchievementCriterionCompletion(AchievementCriterionCompletionJson achievementCriterionCompletionJson) : this()
        {
            Load(achievementCriterionCompletionJson);
        }

        public void Load(AchievementCriterionCompletionJson achievementCriterionCompletionJson)
        {
            BlizzardId = achievementCriterionCompletionJson.Id;
            Amount = achievementCriterionCompletionJson.Amount;
            IsCompleted = achievementCriterionCompletionJson.IsCompleted;
            if (achievementCriterionCompletionJson.ChildCriteria != null && achievementCriterionCompletionJson.ChildCriteria.Any())
            {
                ChildCriteria = new List<AchievementCriterionCompletion>();
                foreach (AchievementCriterionCompletionJson child in achievementCriterionCompletionJson.ChildCriteria)
                    ChildCriteria.Add(new AchievementCriterionCompletion(child));
            }
        }
    }
}