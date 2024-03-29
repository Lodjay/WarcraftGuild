﻿using System;
using System.Collections.Generic;
using System.Linq;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Guild : WoWModel
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string RealmSlug { get; set; }
        public GuildCrest Crest { get; set; }
        public Faction Faction { get; set; }
        public DateTime CreationDate { get; set; }
        public long AchievementCount { get; set; }
        public long AchievementPoints { get; set; }
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
            Load(guildJson);
            LoadAchievements(guildAchievementsJson);
            LoadRoster(guildRosterJson);
        }

        public void Load(GuildJson guildJson)
        {
            if (CheckJson(guildJson))
            {
                BlizzardId = guildJson.Id;
                Name = guildJson.Name;
                Crest = new GuildCrest(guildJson.Crest);
                Faction = guildJson.Faction.Type.ParseCode<Faction>();
                CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(guildJson.CreationTimestamp);
            }
        }

        public void LoadAchievements(GuildAchievementsJson guildAchievementsJson)
        {
            if (CheckJson(guildAchievementsJson))
            {
                AchievementCount = guildAchievementsJson.TotalQuantity;
                AchievementPoints = guildAchievementsJson.TotalPoints;
                if (Achievements.Any())
                    Achievements.Clear();
                foreach (AchievementCompletionJson achievmentCompletionJson in guildAchievementsJson.Achievements)
                    Achievements.Add(new AchievementCompletion(achievmentCompletionJson));
                if (AchievementCategoryCompletion.Any())
                    AchievementCategoryCompletion.Clear();
                foreach (AchievementCategoryCompletionJson achievementCategoryCompletionJson in guildAchievementsJson.CategoriesProgress)
                    AchievementCategoryCompletion.Add(new AchievementCategoryCompletion(achievementCategoryCompletionJson));
            }
        }

        public void LoadRoster(GuildRosterJson guildRosterJson)
        {
            if (CheckJson(guildRosterJson))
            {
                if (Members.Any())
                    Members.Clear();
                foreach (GuildMemberJson member in guildRosterJson.Members)
                    Members.Add(new GuildMember(member));
            }
        }
    }
}