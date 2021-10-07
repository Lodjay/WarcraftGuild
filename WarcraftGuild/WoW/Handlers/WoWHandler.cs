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
                FillRealms(),
                FillConnectedRealms(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        private static async Task<bool> DeleteAllDatas()
        {
            Repository repository = new Repository();
            await repository.DropAll();
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
            Repository repository = new Repository();
            ConnectedRealmJson connectedRealmJson = await _blizzardApiReader.GetAsync<ConnectedRealmJson>(path, Namespace.Dynamic).ConfigureAwait(false);
            await repository.Insert(new ConnectedRealm(connectedRealmJson)).ConfigureAwait(false);
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
            Repository repository = new Repository();
            RealmJson realmJson = await _blizzardApiReader.GetAsync<RealmJson>($"data/wow/realm/{realmSlug}", Namespace.Dynamic).ConfigureAwait(false);
            await repository.Insert(new Realm(realmJson)).ConfigureAwait(false);
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
            Repository repository = new Repository();
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
            await repository.Insert(new Achievement(result)).ConfigureAwait(false);
        }

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
            Repository repository = new Repository();
            AchievementCategoryJson result = await _blizzardApiReader.GetAsync<AchievementCategoryJson>($"data/wow/achievement-category/{achievementCategoryJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                achievementCategoryJson.ResultCode = result.ResultCode;
                result = achievementCategoryJson;
            }
            await repository.Insert(new AchievementCategory(result)).ConfigureAwait(false);
        }

        #endregion Achievements

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