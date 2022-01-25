using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class AchievementCategory : WoWModel
    {
        public string Name { get; set; }
        public bool GuildCategory { get; set; }
        public int Order { get; set; }
        public ulong ParentId { get; set; }
        public List<ulong> AchievementList { get; set; }

        public AchievementCategory()
        {
            AchievementList = new List<ulong>();
        }

        public AchievementCategory(AchievementCategoryJson achievementCategoryJson) : this()
        {
            Load(achievementCategoryJson);
        }

        public void Load(AchievementCategoryJson achievementCategoryJson)
        {
            if (CheckJson(achievementCategoryJson))
            {
                BlizzardId = achievementCategoryJson.Id;
                Name = achievementCategoryJson.Name;
                Order = achievementCategoryJson.Order;
                if (achievementCategoryJson.ParentCategory != null)
                    ParentId = achievementCategoryJson.ParentCategory.Id;
                if (achievementCategoryJson.Achievments != null)
                    foreach (AchievementJson achievment in achievementCategoryJson.Achievments)
                        AchievementList.Add(achievment.Id);
            }
        }
    }
}