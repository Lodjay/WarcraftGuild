using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class AchievementCategoryCompletion
    {
        public ulong CategoryId { get; set; }
        public uint Quantity { get; set; }
        public uint Points { get; set; }

        public AchievementCategoryCompletion()
        {
        }

        public AchievementCategoryCompletion(AchievementCategoryCompletionJson achievementCategoryCompletionJson) : this()
        {
            Load(achievementCategoryCompletionJson);
        }

        public void Load(AchievementCategoryCompletionJson achievementCategoryCompletionJson)
        {
            if (achievementCategoryCompletionJson != null)
            {
                CategoryId = achievementCategoryCompletionJson.Category.Id;
                Quantity = achievementCategoryCompletionJson.Quantity;
                Points = achievementCategoryCompletionJson.Points;
            }
        }
    }
}