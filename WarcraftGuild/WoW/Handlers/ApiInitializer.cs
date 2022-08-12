using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.Core.Handlers;
using WarcraftGuild.Core.Helpers;
using WarcraftGuild.Core.Models;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;
using WarcraftGuild.WoWHeadApi;

namespace WarcraftGuild.WoW.Handlers
{

    public class ApiInitializer : IApiInitializer
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly IWoWHeadApiReader _wowHeadApiReader;
        private readonly IDbManager _dbManager;
        private readonly BlizzardTaskManager _taskManager;

        public ApiInitializer(IBlizzardApiReader blizzardApiReader, IWoWHeadApiReader wowHeadApiReader, IDbManager dbManager)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _wowHeadApiReader = wowHeadApiReader ?? throw new ArgumentNullException(nameof(wowHeadApiReader));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _taskManager = new BlizzardTaskManager(_blizzardApiReader.GetShorterLimiter().Clone());
        }

        public async Task InitAll()
        {
            await InitRealms().ConfigureAwait(false);
            await InitAchievements().ConfigureAwait(false);
            await InitApiDatas().ConfigureAwait(false);
            await InitCharacterDatas().ConfigureAwait(false);
        }


        public async Task InitApiDatas()
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop("LocaleString"),
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            List<Task> InitTasks = new()
            {
                InitLocaleString(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        public async Task InitAchievements()
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop<Achievement>(),
                _dbManager.Drop<AchievementCategory>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            List<Task> InitTasks = new()
            {
                FillAchievements(),
                FillAchievementCategories()
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        public async Task InitRealms()
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop<Realm>(),
                _dbManager.Drop<ConnectedRealm>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            List<Task> InitTasks = new()
            {
                FillRealms(),
                FillConnectedRealms(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        public async Task InitCharacterDatas()
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop<Race>(),
                _dbManager.Drop<Class>(),
                _dbManager.Drop<Specialization>(),
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            List<Task> InitTasks = new()
            {
                FillRaces(),
                FillClasses(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
        }

        public async Task InitGuild(string realmSlug, string guildTag)
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop<Guild>(),
                _dbManager.Drop<Character>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            Guild guild = new()
            {
                Tag = guildTag,
                RealmSlug = realmSlug
            };
            GuildJson guildJson = await _blizzardApiReader.GetAsync<GuildJson>($"data/wow/guild/{realmSlug}/{guildTag}", Namespace.Profile, true).ConfigureAwait(false);
            guild.Load(guildJson);
            GuildAchievementsJson guildAchievementsJson = await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{realmSlug}/{guildTag}/achievements", Namespace.Profile, true).ConfigureAwait(false);
            guild.LoadAchievements(guildAchievementsJson);
            GuildRosterJson guildRosterJson = await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realmSlug}/{guildTag}/roster", Namespace.Profile, true).ConfigureAwait(false);
            if (guildRosterJson != null)
                await FillRoster(guildRosterJson).ConfigureAwait(false);
            guild.LoadRoster(guildRosterJson);
            await _dbManager.Insert(guild).ConfigureAwait(false);
        }

        #region BlizzardAPIDatas

        #region ConnectedRealms

        private async Task FillConnectedRealms()
        {
            ConnectedRealmIndexJson index = await _blizzardApiReader.GetAsync<ConnectedRealmIndexJson>("data/wow/connected-realm/index", Namespace.Dynamic, true).ConfigureAwait(false);
            foreach (HrefJson href in index.ConnectedRealms)
                _taskManager.AddTask(FillConnectedRealm(href.Uri.LocalPath));
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillConnectedRealm(string path)
        {
            ConnectedRealmJson connectedRealmJson = await _blizzardApiReader.GetAsync<ConnectedRealmJson>(path, Namespace.Dynamic, true).ConfigureAwait(false);
            await _dbManager.Insert(new ConnectedRealm(connectedRealmJson)).ConfigureAwait(false);
        }

        #endregion ConnectedRealms

        #region Realms

        private async Task FillRealms()
        {
            RealmIndexJson index = await _blizzardApiReader.GetAsync<RealmIndexJson>("data/wow/realm/index", Namespace.Dynamic, true).ConfigureAwait(false);
            foreach (RealmJson realm in index.Realms)
                _taskManager.AddTask(FillRealm(realm.Slug));
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillRealm(string realmSlug)
        {
            RealmJson realmJson = await _blizzardApiReader.GetAsync<RealmJson>($"data/wow/realm/{realmSlug}", Namespace.Dynamic, true).ConfigureAwait(false);
            await _dbManager.Insert(new Realm(realmJson)).ConfigureAwait(false);
        }

        #endregion Realms

        #region Achievements

        private async Task FillAchievements()
        {
            AchievementIndexJson index = await _blizzardApiReader.GetAsync<AchievementIndexJson>("data/wow/achievement/index", Namespace.Static, true).ConfigureAwait(false);
            foreach (AchievementJson achievement in index.Achievements)
                _taskManager.AddTask(FillAchievement(achievement), 2);
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillAchievement(AchievementJson achievementJson)
        {
            AchievementJson result = await _blizzardApiReader.GetAsync<AchievementJson>($"data/wow/achievement/{achievementJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                achievementJson.ResultCode = result.ResultCode;
                result = achievementJson;
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "Achievement",
                    BlizzardId = achievementJson.Id,
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/achievement/{achievementJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Achievement(result)).ConfigureAwait(false);
        }

        #endregion Achievements

        #region AchievementCategories

        private async Task FillAchievementCategories()
        {
            AchievementCategoryIndexJson index = await _blizzardApiReader.GetAsync<AchievementCategoryIndexJson>("data/wow/achievement-category/index", Namespace.Static, true).ConfigureAwait(false);
            foreach (AchievementCategoryJson categoryJson in index.Categories)
                _taskManager.AddTask(FillAchievementCategory(categoryJson), 2);
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillAchievementCategory(AchievementCategoryJson achievementCategoryJson)
        {
            AchievementCategoryJson result = await _blizzardApiReader.GetAsync<AchievementCategoryJson>($"data/wow/achievement-category/{achievementCategoryJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                achievementCategoryJson.ResultCode = result.ResultCode;
                result = achievementCategoryJson;
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "AchievementCategory",
                    BlizzardId = achievementCategoryJson.Id,
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);
            }
            await _dbManager.Insert(new AchievementCategory(result)).ConfigureAwait(false);
        }

        #endregion AchievementCategories

        #region Races

        private async Task FillRaces()
        {
            RaceIndexJson index = await _blizzardApiReader.GetAsync<RaceIndexJson>("data/wow/playable-race/index", Namespace.Static, true).ConfigureAwait(false);
            foreach (RaceJson race in index.Races)
                _taskManager.AddTask(FillRace(race));
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillRace(RaceJson raceJson)
        {
            RaceJson result = await _blizzardApiReader.GetAsync<RaceJson>($"data/wow/playable-race/{raceJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                raceJson.ResultCode = result.ResultCode;
                result = raceJson;
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "Race",
                    BlizzardId = raceJson.Id,
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Race(result)).ConfigureAwait(false);
        }

        #endregion Races

        #region Classes

        private async Task FillClasses()
        {
            ClassIndexJson index = await _blizzardApiReader.GetAsync<ClassIndexJson>("data/wow/playable-class/index", Namespace.Static, true).ConfigureAwait(false);
            foreach (ClassJson classJson in index.Classes)
                _taskManager.AddTask(FillClass(classJson), 6);
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillClass(ClassJson classJson)
        {
            ClassJson result = await _blizzardApiReader.GetAsync<ClassJson>($"data/wow/playable-class/{classJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                classJson.ResultCode = result.ResultCode;
                result = classJson;
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "Class",
                    BlizzardId = classJson.Id,
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-class/{classJson.Id}", Namespace.Static, true).ConfigureAwait(false);

                List<Task> tasks = new();
                foreach (SpecializationJson specializationJson in result.Specializations)
                    tasks.Add(FillSpecialization(specializationJson));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Class(result)).ConfigureAwait(false);
        }

        private async Task FillSpecialization(SpecializationJson specializationJson)
        {
            SpecializationJson result = await _blizzardApiReader.GetAsync<SpecializationJson>($"data/wow/playable-specialization/{specializationJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            if (result.ResultCode != HttpStatusCode.OK)
            {
                specializationJson.ResultCode = result.ResultCode;
                result = specializationJson;
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "Specialization",
                    BlizzardId = specializationJson.Id,
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);
            }
            else
            {
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-specialization/{specializationJson.Id}", Namespace.Static, true).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Specialization(result)).ConfigureAwait(false);
        }

        #endregion Classes

        #region Characters

        private async Task FillRoster(GuildRosterJson guildRosterJson)
        {
            foreach (GuildMemberJson memberJson in guildRosterJson.Members)
            {
                if (memberJson.Member != null && memberJson.Member.Realm != null)
                {
                    CharacterJson characterJson = new()
                    {
                        Id = memberJson.Member.Id,
                        Name = memberJson.Member.Name,
                        Realm = new RealmJson { Slug = memberJson.Member.Realm.Slug }
                    };
                    _taskManager.AddTask(FillCharacter(characterJson), 3);
                }
            }
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }

        private async Task FillCharacter(CharacterJson characterJson)
        {
            await _dbManager.DeleteByBlizzardId<Character>(characterJson.Id).ConfigureAwait(false);
            CharacterJson result = await _blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}", Namespace.Profile, true).ConfigureAwait(false);
            bool isValid;
            if (result.ResultCode != HttpStatusCode.OK)
            {
                await _dbManager.Log(new LogEvent
                {
                    Message = "Unabled to reach BlizzardAPI",
                    Description = $"Return code {result.ResultCode}",
                    Collection = "Character",
                    BlizzardId = characterJson.Id,
                    KeyData = $"{characterJson.Name}-{characterJson.Realm.Slug}",
                    Severity = LogLevel.Warning
                }).ConfigureAwait(false);

                characterJson.ResultCode = result.ResultCode;
                result = characterJson;
                isValid = true;
            }
            else
            {
                CharacterStatusJson characterStatutJson = await _blizzardApiReader.GetAsync<CharacterStatusJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/status", Namespace.Profile, true).ConfigureAwait(false);
                isValid = characterStatutJson.ResultCode == HttpStatusCode.OK && characterStatutJson.IsValid && characterStatutJson.Id == characterJson.Id;
                if (isValid)
                {
                    result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/character-media", Namespace.Profile, true).ConfigureAwait(false);
                }
                else
                {
                    await _dbManager.Log(new LogEvent
                    {
                        Message = "Character Invalid",
                        Description = $"Blizzard API return invalid status (character may be innactive or deleted)",
                        Collection = "Character",
                        BlizzardId = characterJson.Id,
                        KeyData = $"{characterJson.Name}-{characterJson.Realm.Slug}",
                        Severity = LogLevel.Warning
                    }).ConfigureAwait(false);
                    await _dbManager.DeleteByBlizzardId<Character>(characterJson.Id).ConfigureAwait(false);
                }
            }
            if (isValid)
                await _dbManager.Insert(new Character(result)).ConfigureAwait(false);
        }

        #endregion Characters

        #endregion

        #region WoWGuildApiDatas

        #region LocaleStrings

        public async Task InitLocaleString()
        {
            foreach (LocaleString locale in LocaleStringInitializer.Generate())
                _taskManager.AddTask(_dbManager.Insert(locale));
            await _taskManager.RunTaskes().ConfigureAwait(false);
        }
        #endregion

        #endregion
    }
}