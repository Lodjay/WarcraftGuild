using System.Collections.Generic;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IBlizzardApiHandler
    {
        Task<ConnectedRealm> GetConnectedRealmById(ulong blizzardId, bool forceUpdate = false);
        Task<Realm> GetRealmBySlug(string slug, bool forceUpdate = false);
        Task<Guild> GetGuildByTag(string realmSlug, string guildTag, bool forceUpdate = false);
    }
}