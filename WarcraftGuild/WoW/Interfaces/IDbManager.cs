using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IDbManager
    {
        #region Generic
        Task Insert<T>(T data) where T : WoWModel, new();
        Task<List<T>> GetAll<T>() where T : WoWModel, new();
        Task<T> GetByGuid<T>(Guid id) where T : WoWModel, new();
        Task<T> GetByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new();
        Task DeleteAll<T>() where T : WoWModel, new();
        Task Delete<T>(T data) where T : WoWModel, new();
        Task DeleteById<T>(Guid id) where T : WoWModel, new();
        Task DeleteByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new();
        Task Drop<T>() where T : WoWModel, new();
        Task Drop(string collection);
        Task DropAll();
        #endregion

        #region Specific
        Task<Realm> GetRealmBySlug(string slug);
        #endregion
    }
}