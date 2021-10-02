using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IWoWHandler
    {

        Task<Guild> GetGuild(string realmName, string guildName);

        Task<Guild> GetGuild(string realmName, string guildName, bool forceRefresh);
    }
}
