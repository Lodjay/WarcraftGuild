using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.WoWJson;
using WarcraftGuild.Core.Enums;
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
                GuildActivityJson guildActivityJson = await _blizzardApiReader.GetAsync<GuildActivityJson>($"data/wow/guild/{realmName}/{guildName}/activity ", Namespace.Profile).ConfigureAwait(false);
                GuildRosterJson guildRosterJson = await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realmName}/{guildName}/roster ", Namespace.Profile).ConfigureAwait(false);
                guild.Load(guildJson, guildAchievementsJson, guildActivityJson, guildRosterJson);
            }
            return guild;
        }
    }
}
