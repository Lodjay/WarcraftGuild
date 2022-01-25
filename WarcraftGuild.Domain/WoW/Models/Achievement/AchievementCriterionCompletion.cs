using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class AchievementCriterionCompletion
    {
        public ulong BlizzardId { get; set; }
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