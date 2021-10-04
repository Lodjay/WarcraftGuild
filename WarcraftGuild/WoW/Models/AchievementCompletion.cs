using System;
using System.Net;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class AchievementCompletion : WoWData
    {
        public DateTime? CompletionDate { get; set; }
        public AchievementCriterionCompletion Criteria { get; set; }

        public AchievementCompletion()
        {
        }

        public AchievementCompletion(AchievmentCompletionJson achievmentCompletionJson) : base()
        {
            Load(achievmentCompletionJson);
        }

        public void Load(AchievmentCompletionJson achievmentCompletionJson)
        {
            if (CanLoadJson(achievmentCompletionJson))
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
    }
}