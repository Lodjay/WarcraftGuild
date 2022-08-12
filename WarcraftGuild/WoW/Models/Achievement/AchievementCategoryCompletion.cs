using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCategoryCompletion
    {
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong CategoryId { get; set; }
        public long Quantity { get; set; }
        public long Points { get; set; }

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