using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCategoryCompletion : WoWModel
    {
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
            if (CheckJson(achievementCategoryCompletionJson))
            {
                BlizzardId = achievementCategoryCompletionJson.Category.Id;
                Quantity = achievementCategoryCompletionJson.Quantity;
                Points = achievementCategoryCompletionJson.Points;
            }
        }
    }
}