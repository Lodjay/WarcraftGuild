﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCriterionCompletion
    {
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong BlizzardId { get; set; }
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong Amount { get; set; }
        public bool IsCompleted { get; set; }
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
            if (achievementCriterionCompletionJson != null)
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
}