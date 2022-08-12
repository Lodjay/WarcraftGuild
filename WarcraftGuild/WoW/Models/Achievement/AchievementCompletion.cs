using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCompletion
    {
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong AchievementId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public AchievementCriterionCompletion Criteria { get; set; }

        public AchievementCompletion()
        {
        }

        public AchievementCompletion(AchievementCompletionJson achievmentCompletionJson) : base()
        {
            Load(achievmentCompletionJson);
        }

        public void Load(AchievementCompletionJson achievmentCompletionJson)
        {
            if (achievmentCompletionJson != null)
            {
                AchievementId = achievmentCompletionJson.Id;
                if (achievmentCompletionJson.CompletedTimestamp.HasValue)
                    CompletionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(achievmentCompletionJson.CompletedTimestamp.Value);
                else
                    CompletionDate = null;
                if (achievmentCompletionJson.Criteria != null)
                    Criteria = new AchievementCriterionCompletion(achievmentCompletionJson.Criteria);
            }
        }
    }
}