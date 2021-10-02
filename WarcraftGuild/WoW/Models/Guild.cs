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
        public ulong BlizzardId { get; private set; }
        public string Name { get; private set; }
        public GuildCrest Crest { get; private set; }
        public Faction Faction { get; private set; }
        public DateTime CreationDate { get; private set; }
        public uint AchievementCount { get; private set; }
        public uint AchievementPoints { get; private set; }
        public List<AchievementCompletion> Achievements { get; private set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; private set; }
        public List<GuildMember> Members { get; private set; }

        public Guild()
        {
            Achievements = new List<AchievementCompletion>();
            AchievementCategoryCompletion = new List<AchievementCategoryCompletion>();
            Members = new List<GuildMember>();
        }

        public Guild(GuildJson guildJson, GuildAchievementsJson guildAchievementsJson, GuildRosterJson guildRosterJson) : this()
        {
            Load(guildJson, guildAchievementsJson, guildRosterJson);
        }

        public void Load(GuildJson guildJson, GuildAchievementsJson guildAchievementsJson,  GuildRosterJson guildRosterJson)
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
            if (AchievementCategoryCompletion.Any())
                AchievementCategoryCompletion.Clear();
            foreach (AchievementCategoryCompletionJson achievementCategoryCompletionJson in guildAchievementsJson.CategoriesProgress)
                AchievementCategoryCompletion.Add(new AchievementCategoryCompletion(achievementCategoryCompletionJson));
            if (Members.Any())
                Members.Clear();
            foreach (GuildMemberJson member in guildRosterJson.Members)
                Members.Add(new GuildMember(member));
        }
    }
}