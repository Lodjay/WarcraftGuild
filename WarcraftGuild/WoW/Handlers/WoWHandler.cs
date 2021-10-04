using System;
using System.Collections.Generic;
using System.Linq;
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
                GuildAchievementsJson guildAchievementsJson = await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{realmName}/{guildName}/achievements ", Namespace.Profile).ConfigureAwait(false);
                GuildRosterJson guildRosterJson = await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realmName}/{guildName}/roster ", Namespace.Profile).ConfigureAwait(false);
                if (guildRosterJson != null)
                {
                    List<Task<CharacterJson>> tasks = new List<Task<CharacterJson>>();
                    foreach (GuildMemberJson member in guildRosterJson.Members)
                        if (member.Member != null)
                            tasks.Add(_blizzardApiReader.GetAsync<CharacterJson>($"profile/wow/character/{realmName}/{member.Member.Name.ToLower()}", Namespace.Profile));
                    CharacterJson[] results = await Task.WhenAll(tasks);
                    foreach (CharacterJson result in results)
                        if (result.Id > 0)
                            guildRosterJson.Members.FirstOrDefault(x => x.Member != null && x.Member.Id == result.Id).Member.Character = result;
                }
                guild.Load(guildJson, guildAchievementsJson, guildRosterJson);
                await Repository.Delete(guild);
                await Repository.Insert(guild);
            }

            return guild;
        }
    }
}