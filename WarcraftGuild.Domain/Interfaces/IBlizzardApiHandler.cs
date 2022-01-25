using WarcraftGuild.Domain.WoW.Models;

namespace WarcraftGuild.Domain.Interfaces
{
    public interface IBlizzardApiHandler
    {
        Task<ConnectedRealm> GetConnectedRealmById(ulong blizzardId, bool forceUpdate = false);

        Task<Realm> GetRealmBySlug(string slug, bool forceUpdate = false);

        Task<Guild> GetGuildBySlug(string realmSlug, string guildSlug, bool forceUpdate = false);
    }
}