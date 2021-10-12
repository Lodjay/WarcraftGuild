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
    public class ApiInitializer : IApiInitializer
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly IDbManager _dbManager;

        public ApiInitializer(IBlizzardApiReader blizzardApiReader, IDbManager dbManager)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }

        public async Task InitGuild(string realmSlug, string guildSlug)
        {
            List<Task> DropTasks = new List<Task>
            {
                _dbManager.Drop<Guild>(),
                _dbManager.Drop<Character>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            Guild guild = new Guild
            {
                Slug = guildSlug,
                RealmSlug = realmSlug
            };
            GuildJson guildJson = await _blizzardApiReader.GetAsync<GuildJson>($"data/wow/guild/{realmSlug}/{guildSlug}", Namespace.Profile).ConfigureAwait(false);
            guild.Load(guildJson);
            GuildAchievementsJson guildAchievementsJson = await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{realmSlug}/{guildSlug}/achievements", Namespace.Profile).ConfigureAwait(false);
            guild.Load(guildAchievementsJson);
            GuildRosterJson guildRosterJson = await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realmSlug}/{guildSlug}/roster", Namespace.Profile).ConfigureAwait(false);
            if (guildRosterJson != null)
                await FillRoster(guildRosterJson).ConfigureAwait(false);
            guild.Load(guildRosterJson);
            await _dbManager.Insert(guild).ConfigureAwait(false);
        }

        public async Task Init()
        {
            List<Task> DropTasks = new List<Task>
            {
                _dbManager.Drop<Achievement>(),
                _dbManager.Drop<AchievementCategory>(),
                _dbManager.Drop<Realm>(),
                _dbManager.Drop<ConnectedRealm>(),
                _dbManager.Drop<Race>(),
                _dbManager.Drop<Class>(),
                _dbManager.Drop<Specialization>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            List<Task> InitTasks = new List<Task>
            {
                FillAchievements(),
                FillAchievementCategories(),
                FillRealms(),
                FillConnectedRealms(),
                FillRaces(),
                FillClasses()
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
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
            RaceJson result = await _blizzardApiReader.GetAsync<RaceJson>($"data/wow/playable-race/{raceJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                raceJson.ResultCode = result.ResultCode;
                result = raceJson;
            }
            await _dbManager.Insert(new Race(result)).ConfigureAwait(false);
        }

        #endregion

        #region Classes

        private async Task FillClasses()
        {
            List<Task> tasks = new List<Task>();
            ClassIndexJson index = await _blizzardApiReader.GetAsync<ClassIndexJson>("data/wow/playable-class/index", Namespace.Static).ConfigureAwait(false);
            foreach (ClassJson classJson in index.Classes)
                tasks.Add(FillClass(classJson));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillClass(ClassJson classJson)
        {
            ClassJson result = await _blizzardApiReader.GetAsync<ClassJson>($"data/wow/playable-class/{classJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                classJson.ResultCode = result.ResultCode;
                result = classJson;
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-class/{classJson.Id}", Namespace.Static).ConfigureAwait(false);

                List<Task> subTasks = new List<Task>(); 
                foreach (SpecializationJson specializationJson in result.Specializations)
                    subTasks.Add(FillSpecialization(specializationJson));
                await Task.WhenAll(subTasks).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Class(result)).ConfigureAwait(false);
        }

        private async Task FillSpecialization(SpecializationJson specializationJson)
        {
            SpecializationJson result = await _blizzardApiReader.GetAsync<SpecializationJson>($"data/wow/playable-specialization/{specializationJson.Id}", Namespace.Static).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                specializationJson.ResultCode = result.ResultCode;
                result = specializationJson;
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-specialization/{specializationJson.Id}", Namespace.Static).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Specialization(result)).ConfigureAwait(false);
        }

        #endregion

        #region Characters
        private async Task FillRoster(GuildRosterJson guildRosterJson)
        {
            List<Task> tasks = new List<Task>();
            foreach (GuildMemberJson memberJson in guildRosterJson.Members)
            {
                if (memberJson.Member != null && memberJson.Member.Realm != null)
                {
                    CharacterJson characterJson = new CharacterJson
                    {
                        Id = memberJson.Member.Id,
                        Name = memberJson.Member.Name,
                        Realm = new RealmJson { Slug = memberJson.Member.Realm.Slug }
                    };
                    tasks.Add(FillCharacter(characterJson));
                }
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillCharacter(CharacterJson characterJson)
        {
            await _dbManager.DeleteByBlizzardId<Character>(characterJson.Id).ConfigureAwait(false);
            CharacterJson result = await _blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}", Namespace.Profile).ConfigureAwait(false);
            CharacterStatusJson characterStatutJson = await _blizzardApiReader.GetAsync<CharacterStatusJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/status", Namespace.Profile).ConfigureAwait(false);
            bool isValid = characterStatutJson.ResultCode == HttpStatusCode.OK && characterStatutJson.IsValid && characterStatutJson.Id == characterJson.Id;
            if (result.ResultCode != HttpStatusCode.OK || !isValid)
            {
                characterJson.ResultCode = isValid? result.ResultCode : HttpStatusCode.Forbidden;
                result = characterJson;
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/character-media", Namespace.Profile).ConfigureAwait(false);
            }
            if (isValid)
                await _dbManager.Insert(new Character(result)).ConfigureAwait(false);
        }
        #endregion
    }
}