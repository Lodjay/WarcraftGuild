using WarcraftGuild.Domain.Core.Model;
using WarcraftGuild.Domain.WoW.Models;

namespace WarcraftGuild.Domain.Interfaces
{
    public interface IDbManager
    {
        #region Generic

        Task Insert<T>(T data) where T : WoWModel, new();

        Task<List<T>> GetAll<T>() where T : WoWModel, new();

        Task<T> GetByGuid<T>(Guid id) where T : WoWModel, new();

        Task<T> GetByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new();

        Task Delete<T>(T data) where T : WoWModel, new();

        Task DeleteById<T>(Guid id) where T : WoWModel, new();

        Task DeleteByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new();

        Task Drop<T>() where T : WoWModel, new();

        Task Drop(string collection);

        Task DropAll();

        #endregion Generic

        #region Logging

        Task Log(LogEvent log);

        #endregion Logging

        #region Specific

        Task<Realm> GetRealmBySlug(string slug);

        Task<Guild> GetGuildBySlug(string realmSlug, string guildSlug);

        #endregion Specific

        #region NativeDatas

        Task Insert(LocaleString locale);

        #endregion NativeDatas
    }
}