using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
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

        public ApiInitializer(IBlizzardApiReader blizzardApiReader, IWoWHeadApiReader wowHeadApiReader, IDbManager dbManager)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _wowHeadApiReader = wowHeadApiReader ?? throw new ArgumentNullException(nameof(wowHeadApiReader));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }

        public async Task InitAll()
        {
            List<Task> InitTasks = new()
            {
                InitRealms(),
                InitAchievements(),
                InitApiDatas(),
                InitCharacterDatas(),
            };
            await Task.WhenAll(InitTasks).ConfigureAwait(false);
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

        public async Task InitGuild(string realmSlug, string guildSlug)
        {
            List<Task> DropTasks = new()
            {
                _dbManager.Drop<Guild>(),
                _dbManager.Drop<Character>()
            };
            await Task.WhenAll(DropTasks).ConfigureAwait(false);
            Guild guild = new()
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

        #region BlizzardAPIDatas

        #region ConnectedRealms

        private async Task FillConnectedRealms()
        {
            List<Task> tasks = new();
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
            List<Task> tasks = new();
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
            List<Task> tasks = new();
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
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/achievement/{achievementJson.Id}", Namespace.Static).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Achievement(result)).ConfigureAwait(false);
        }

        #endregion Achievements

        #region AchievementCategories

        private async Task FillAchievementCategories()
        {
            List<Task> tasks = new();
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
            List<Task> tasks = new();
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
            List<Task> tasks = new();
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
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-class/{classJson.Id}", Namespace.Static).ConfigureAwait(false);

                List<Task> subTasks = new();
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
                result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"data/wow/media/playable-specialization/{specializationJson.Id}", Namespace.Static).ConfigureAwait(false);
            }
            await _dbManager.Insert(new Specialization(result)).ConfigureAwait(false);
        }

        #endregion Classes

        #region Characters

        private async Task FillRoster(GuildRosterJson guildRosterJson)
        {
            List<Task> tasks = new();
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
                    tasks.Add(FillCharacter(characterJson));
                }
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task FillCharacter(CharacterJson characterJson)
        {
            await _dbManager.DeleteByBlizzardId<Character>(characterJson.Id).ConfigureAwait(false);
            CharacterJson result = await _blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}", Namespace.Profile).ConfigureAwait(false);
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
                CharacterStatusJson characterStatutJson = await _blizzardApiReader.GetAsync<CharacterStatusJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/status", Namespace.Profile).ConfigureAwait(false);
                isValid = characterStatutJson.ResultCode == HttpStatusCode.OK && characterStatutJson.IsValid && characterStatutJson.Id == characterJson.Id;
                if (isValid)
                {
                    result.Media = await _blizzardApiReader.GetAsync<MediaJson>($"profile/wow/character/{characterJson.Realm.Slug}/{characterJson.Name.ToLower()}/character-media", Namespace.Profile).ConfigureAwait(false);
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
            List<Task> tasks = new();
            foreach (LocaleString locale in LocaleStringInitializer.Generate())
                tasks.Add(_dbManager.Insert(locale));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        #endregion

        #endregion
    }
}