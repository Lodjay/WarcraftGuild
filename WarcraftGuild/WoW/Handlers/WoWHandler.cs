using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.WoW.Configuration;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Handlers
{
    public class WoWHandler : IWoWHandler
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly IDbManager _dbManager;

        public WoWHandler(IBlizzardApiReader blizzardApiReader, IDbManager dbManager)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }

        public async Task<Guild> GetGuild(string realmName, string guildName)
        {
            return await GetGuild(realmName, guildName, false).ConfigureAwait(false);
        }

        public async Task<Guild> GetGuild(string realmName, string guildName, bool forceRefresh)
        {
            Guild guild = new Guild();
            if (forceRefresh)
            {
                GuildJson guildJson = await _blizzardApiReader.GetAsync<GuildJson>($"data/wow/guild/{realmName}/{guildName} ", Namespace.Profile).ConfigureAwait(false);
                guild.Load(guildJson);
                GuildAchievementsJson guildAchievementsJson = await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{realmName}/{guildName}/achievements ", Namespace.Profile).ConfigureAwait(false);
                guild.Load(guildAchievementsJson);
                GuildRosterJson guildRosterJson = await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realmName}/{guildName}/roster ", Namespace.Profile).ConfigureAwait(false);
                if (guildRosterJson != null)
                {
                    List<Task<CharacterJson>> tasks = new List<Task<CharacterJson>>();
                    foreach (GuildMemberJson member in guildRosterJson.Members)
                        if (member.Member != null && member.Member.Realm != null)
                            tasks.Add(CompleteCharacter(member.Member));
                    CharacterJson[] results = await Task.WhenAll(tasks);
                    foreach (CharacterJson result in results)
                        guildRosterJson.Members.FirstOrDefault(x => x.Member != null && x.Member.Name == result.Name && x.Member.Realm.Slug == result.Realm.Slug).Member = result;
                }
                guild.Load(guildRosterJson);
            }

            return guild;
        }

        public async Task Init()
        {
            await DeleteAllDatas().ConfigureAwait(false);
            List<Task> InitTasks = new List<Task>
            {
                FillAchievements(),
                FillAchievementCategories(),
                FillRealms(),
                FillConnectedRealms(),
                FillRaces(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        private async Task<bool> DeleteAllDatas()
        {
            await _dbManager.DropAll();
            return true;
        }

        #region ConnectedRealms

        private async Task FillConnectedRealms()
        {
            List<Task> tasks = new List<Task>();
            ConnectedRealmIndexJson index = await _blizzardApiReader.GetAsync<ConnectedRealmIndexJson>("data/wow/connected-realm/index", Namespace.Dynamic).ConfigureAwait(false);
            foreach (HrefJson href in index.ConnectedRealms)
                tasks.Add(FillConnectedRealm(href.Uri.LocalPath));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillConnectedRealm(string path)
        {
            ConnectedRealmJson connectedRealmJson = await _blizzardApiReader.GetAsync<ConnectedRealmJson>(path, Namespace.Dynamic).ConfigureAwait(false);
            await _dbManager.Insert(new ConnectedRealm(connectedRealmJson)).ConfigureAwait(false);
        }

        #endregion ConnectedRealms

        #region Realms

        private async Task FillRealms()
        {
            List<Task> tasks = new List<Task>();
            RealmIndexJson index = await _blizzardApiReader.GetAsync<RealmIndexJson>("data/wow/realm/index", Namespace.Dynamic).ConfigureAwait(false);
            foreach (RealmJson realm in index.Realms)
                tasks.Add(FillRealm(realm.Slug));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillRealm(string realmSlug)
        {
            RealmJson realmJson = await _blizzardApiReader.GetAsync<RealmJson>($"data/wow/realm/{realmSlug}", Namespace.Dynamic).ConfigureAwait(false);
            await _dbManager.Insert(new Realm(realmJson)).ConfigureAwait(false);
        }

        #endregion Realms

        #region Achievements

        private async Task FillAchievements()
        {
            List<Task> tasks = new List<Task>();
            AchievementIndexJson index = await _blizzardApiReader.GetAsync<AchievementIndexJson>("data/wow/achievement/index", Namespace.Static).ConfigureAwait(false);
            foreach (AchievementJson achievement in index.Achievements)
                tasks.Add(FillAchievement(achievement));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillAchievement(AchievementJson achievementJson)
        {
            AchievementJson result = await _blizzardApiReader.GetAsync<AchievementJson>($"data/wow/achievement/{achievementJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                achievementJson.ResultCode = result.ResultCode;
                result = achievementJson;
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/achievement/{achievementJson.Id}", Namespace.Static).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Achievement(result)).ConfigureAwait(false);
        }

        #endregion

        #region AchievementCategories

        private async Task FillAchievementCategories()
        {
            List<Task> tasks = new List<Task>();
            AchievementCategoryIndexJson index = await _blizzardApiReader.GetAsync<AchievementCategoryIndexJson>("data/wow/achievement-category/index", Namespace.Static).ConfigureAwait(false);
            foreach (AchievementCategoryJson categoryJson in index.Categories)
                tasks.Add(FillAchievementCategory(categoryJson));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillAchievementCategory(AchievementCategoryJson achievementCategoryJson)
        {
            AchievementCategoryJson result = await _blizzardApiReader.GetAsync<AchievementCategoryJson>($"data/wow/achievement-category/{achievementCategoryJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                achievementCategoryJson.ResultCode = result.ResultCode;
                result = achievementCategoryJson;
            }
            await _dbManager.Insert(new AchievementCategory(result)).ConfigureAwait(false);
        }

        #endregion


        #region Races

        private async Task FillRaces()
        {
            List<Task> tasks = new List<Task>();
            RaceIndexJson index = await _blizzardApiReader.GetAsync<RaceIndexJson>("data/wow/playable-race/index", Namespace.Static).ConfigureAwait(false);
            foreach (RaceJson race in index.Races)
                tasks.Add(FillRace(race));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillRace(RaceJson raceJson)
        {
            RaceJson result = await _blizzardApiReader.GetAsync<RaceJson>($"data/wow/race/{raceJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                raceJson.ResultCode = result.ResultCode;
                result = raceJson;
            }
            await _dbManager.Insert(new Race(result)).ConfigureAwait(false);
        }

        #endregion

        private async Task<CharacterJson> CompleteCharacter(CharacterJson character)
        {
            CharacterJson result = await _blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{character.Realm.Slug}/{character.Name.ToLower()}", Namespace.Profile).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                character.ResultCode = result.ResultCode;
                return character;
            }
            else
                return result;
        }
    }
}