using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Configuration;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Handlers
{
    public class DbManager : IDbManager
    {
        private readonly ApiConfiguration _config;
        private readonly IMongoDatabase _db;

        public DbManager(IOptions<ApiConfiguration> apiConfiguration)
        {
            _config = apiConfiguration.Value ?? throw new ArgumentNullException(nameof(apiConfiguration));
            MongoClient client = new MongoClient();
            _db = client.GetDatabase(_config.DataBaseName);
        }

        #region General
        public async Task Insert<T>(T data) where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            data.UpdateDate = DateTime.Now;
            await collection.InsertOneAsync(data).ConfigureAwait(false);
        }

        public async Task<List<T>> GetAll<T>() where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var result = await collection.FindAsync(new BsonDocument()).ConfigureAwait(false);
            return result.ToList();
        }

        public async Task<T> GetByGuid<T>(Guid id) where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
            var result = await collection.FindAsync(filter).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<T> GetByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("BlizzardId", blizzardId);
            var result = await collection.FindAsync(filter).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task DeleteAll<T>() where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            await collection.DeleteManyAsync(filter);
        }

        public async Task Delete<T>(T data) where T : WoWModel, new()
        {
            await DeleteByBlizzardId<T>(data.BlizzardId).ConfigureAwait(false);
        }

        public async Task DeleteById<T>(Guid id) where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task DeleteByBlizzardId<T>(ulong blizzardId) where T : WoWModel, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("BlizzardId", blizzardId);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Drop<T>() where T : WoWModel, new()
        {
            await _db.DropCollectionAsync(typeof(T).Name);
        }

        public async Task Drop(string collection)
        {
            await _db.DropCollectionAsync(collection);
        }

        public async Task DropAll()
        {
            var collections = await _db.ListCollectionNamesAsync().ConfigureAwait(false);
            List<Task> tasks = new List<Task>();
            foreach (string collection in collections.ToList())
                tasks.Add(Drop(collection));
            await Task.WhenAll(tasks);
        }
        #endregion

        #region Specific
        public async Task<Realm> GetRealmBySlug(string slug)
        {
            var collection = _db.GetCollection<Realm>(typeof(Realm).Name);
            FilterDefinition<Realm> filter = Builders<Realm>.Filter.Eq("Slug", slug);
            var result = await collection.FindAsync(filter).ConfigureAwait(false);
            return result.FirstOrDefault();
        }
        #endregion
    }
}