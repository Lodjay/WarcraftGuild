using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCategory : WoWModel
    {
        public string Name { get; set; }
        public bool GuildCategory { get; set; }
        public int Order { get; set; }
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong ParentId { get; set; }
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
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