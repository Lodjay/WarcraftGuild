using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Guild : WoWData
    {
        public string Name { get; set; }
        public GuildCrest Crest { get; set; }
        public Faction Faction { get; set; }
        public DateTime CreationDate { get; set; }
        public uint AchievementCount { get; set; }
        public uint AchievementPoints { get; set; }
        public List<AchievementCompletion> Achievements { get; set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; set; }
        public List<GuildMember> Members { get; set; }

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
            if (guildJson != null)
            {
                BlizzardId = guildJson.Id;
                Name = guildJson.Name;
                Crest = new GuildCrest(guildJson.Crest);
                Faction = guildJson.Faction.Type.ParseCode<Faction>();
                CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(guildJson.CreationTimestamp);
                if (guildAchievementsJson != null)
                {
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
                }
                if (guildRosterJson != null)
                {
                    if (Members.Any())
                        Members.Clear();
                    foreach (GuildMemberJson member in guildRosterJson.Members)
                        if (member.Member != null)
                        {
                            Members.Add(new GuildMember(member));
                        }
                }
            }
        }
    }
}