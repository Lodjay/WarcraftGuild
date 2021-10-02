using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.WoWJson;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Guild
    {
        public long BlizzardId { get; private set; }
        public string Name { get; private set; }
        public GuildCrest Crest { get; private set; }
        public Faction Faction { get; private set; }
        public DateTime CreationDate { get; private set; }
        public int AchievementCount { get; private set; }
        public int AchievementPoints { get; private set; }
        public List<AchievementCompletion> Achievements { get; private set; }

        public Guild()
        {
            Achievements = new List<AchievementCompletion>();
        }

        public Guild(GuildJson guildJson, GuildAchievementsJson guildAchievementsJson, GuildActivityJson guildActivityJson, GuildRosterJson guildRosterJson)
        {
            Achievements = new List<AchievementCompletion>();
            Load(guildJson, guildAchievementsJson, guildActivityJson, guildRosterJson);
        }

        public void Load(GuildJson guildJson, GuildAchievementsJson guildAchievementsJson, GuildActivityJson guildActivityJson, GuildRosterJson guildRosterJson)
        {
            BlizzardId = guildJson.Id;
            Name = guildJson.Name;
            Crest = new GuildCrest(guildJson.Crest);
            Faction = guildJson.Faction.Type.ParseCode<Faction>();
            CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(guildJson.CreationTimestamp);
            AchievementCount = guildAchievementsJson.TotalQuantity;
            AchievementPoints = guildAchievementsJson.TotalPoints;
            if (Achievements.Any())
                Achievements.Clear();
            foreach (AchievmentCompletionJson achievmentCompletionJson in guildAchievementsJson.Achievements)
                Achievements.Add(new AchievementCompletion(achievmentCompletionJson));
        }
    }
}