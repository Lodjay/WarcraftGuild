using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Helpers;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Handlers
{
    public class WoWHandler : IWoWHandler
    {
        private readonly IBlizzardApiReader _blizzardApiReader;

        public WoWHandler(IBlizzardApiReader blizzardApiReader)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
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
                FillRealms()
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        private static async Task<bool> DeleteAllDatas()
        {
            Repository repository = new Repository();
            await repository.DropAll();
            return true;
        }

        #region Realms
        private async Task FillRealms()
        {
            List<ConnectedRealm> connectedRealms = await GetConnectedRealms().ConfigureAwait(false);
            List<Realm> realms = await GetRealms(connectedRealms).ConfigureAwait(false);

            List<Task> insertTasks = new List<Task>();
            Repository repository = new Repository();
            foreach (ConnectedRealm connectedRealm in connectedRealms)
                insertTasks.Add(repository.Insert(connectedRealm));
            foreach (Realm realm in realms)
                insertTasks.Add(repository.Insert(realm));
            await Task.WhenAll(insertTasks).ConfigureAwait(false);
        }


        private async Task<List<ConnectedRealm>> GetConnectedRealms()
        {
            List<ConnectedRealm> connectedRealms = new List<ConnectedRealm>();
            List<Task<ConnectedRealmJson>> CRTasks = new List<Task<ConnectedRealmJson>>();
            ConnectedRealmIndexJson CRList = await _blizzardApiReader.GetAsync<ConnectedRealmIndexJson>("data/wow/connected-realm/", Namespace.Dynamic).ConfigureAwait(false);
            foreach (HrefJson href in CRList.ConnectedRealms)
                CRTasks.Add(GetConnectedRealmJson(href.Uri.LocalPath));
            ConnectedRealmJson[] connectedRealmJsonList = await Task.WhenAll(CRTasks).ConfigureAwait(false);
            foreach (ConnectedRealmJson connectedRealmJson in connectedRealmJsonList)
                connectedRealms.Add(new ConnectedRealm(connectedRealmJson));
            return connectedRealms;
        }

        private async Task<List<Realm>> GetRealms(List<ConnectedRealm> connectedRealms)
        {
            List<Realm> realms = new List<Realm>();
            List<Task<RealmJson>> RTasks = new List<Task<RealmJson>>();
            foreach (ConnectedRealm cr in connectedRealms)
                foreach (string realmSlug in cr.RealmSlugs)
                    RTasks.Add(GetRealmJson(realmSlug));
            RealmJson[] RealmJsonList = await Task.WhenAll(RTasks).ConfigureAwait(false);
            foreach (RealmJson realmJson in RealmJsonList)
                realms.Add(new Realm(realmJson));
            return realms;
        }

        private async Task<ConnectedRealmJson> GetConnectedRealmJson(string path)
        {
            return await _blizzardApiReader.GetAsync<ConnectedRealmJson>(path, Namespace.Dynamic).ConfigureAwait(false);
        }

        private async Task<RealmJson> GetRealmJson(string slug)
        {
            return await _blizzardApiReader.GetAsync<RealmJson>($"data/wow/realm/{slug}", Namespace.Dynamic).ConfigureAwait(false);
        }
        #endregion

        #region Achievements

        private async Task FillAchievements()
        {
            List<Achievement> achievements = await GetAchievements().ConfigureAwait(false);

            List<Task> insertTasks = new List<Task>(); Repository repository = new Repository();
            foreach (Achievement achievement in achievements)
                insertTasks.Add(repository.Insert(achievement));
            await Task.WhenAll(insertTasks).ConfigureAwait(false);
        }

        private async Task FillAchievementCategories()
        {
            List<AchievementCategory> achievementCategories = await GetAchievementCategories().ConfigureAwait(false);

            List<Task> insertTasks = new List<Task>(); Repository repository = new Repository();
            foreach (AchievementCategory achievementCategory in achievementCategories)
                insertTasks.Add(repository.Insert(achievementCategory));
            await Task.WhenAll(insertTasks).ConfigureAwait(false);
        }

        public async Task<List<Achievement>> GetAchievements()
        {
            List<Achievement> achievements = new List<Achievement>();
            List<Task<AchievementJson>> ATasks = new List<Task<AchievementJson>>();
            AchievementIndexJson AList = await _blizzardApiReader.GetAsync<AchievementIndexJson>("data/wow/achievement/index", Namespace.Static).ConfigureAwait(false);
            foreach (AchievementJson achievement in AList.Achievements)
                ATasks.Add(CompleteAchievement(achievement));
            AchievementJson[] AchievementJsonList = await Task.WhenAll(ATasks).ConfigureAwait(false);
            foreach (AchievementJson achievementJson in AchievementJsonList)
                achievements.Add(new Achievement(achievementJson));
            return achievements;
        }

        public async Task<AchievementJson> CompleteAchievement(AchievementJson achievementJson)
        {
            AchievementJson result = await _blizzardApiReader.GetAsync<AchievementJson>($"data/wow/achievement/{achievementJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode == HttpStatusCode.OK)
                return result;
            else
            {
                achievementJson.ResultCode = result.ResultCode;
                return achievementJson;
            }
        }

        public async Task<List<AchievementCategory>> GetAchievementCategories()
        {
            List<AchievementCategory> achievementCategories = new List<AchievementCategory>();
            List<Task<AchievementCategoryJson>> ACTasks = new List<Task<AchievementCategoryJson>>();
            AchievementCategoryIndexJson ACList = await _blizzardApiReader.GetAsync<AchievementCategoryIndexJson>("data/wow/achievement-category/index", Namespace.Static).ConfigureAwait(false);
            foreach (AchievementCategoryJson category in ACList.Categories)
                ACTasks.Add(CompleteAchievementCategory(category));
            AchievementCategoryJson[] AchievementCategoryJsonList = await Task.WhenAll(ACTasks).ConfigureAwait(false);
            foreach (AchievementCategoryJson achievementCategoryJson in AchievementCategoryJsonList)
                achievementCategories.Add(new AchievementCategory(achievementCategoryJson));
            return achievementCategories;
        }

        public async Task<AchievementCategoryJson> CompleteAchievementCategory(AchievementCategoryJson categoryJson)
        {
            AchievementCategoryJson result = await _blizzardApiReader.GetAsync<AchievementCategoryJson>($"data/wow/achievement-category/{categoryJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode == HttpStatusCode.OK)
                return result;
            else
            {
                categoryJson.ResultCode = result.ResultCode;
                return categoryJson;
            }
        }

        #endregion

        private async Task<CharacterJson> CompleteCharacter(CharacterJson character)
        {
            CharacterJson result = await _blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{character.Realm.Slug}/{character.Name.ToLower()}", Namespace.Profile).ConfigureAwait(false);
            if (result.ResultCode == HttpStatusCode.OK)
                return result;
            else
            {
                character.ResultCode = result.ResultCode;
                return character;
            }
        }
    }
}