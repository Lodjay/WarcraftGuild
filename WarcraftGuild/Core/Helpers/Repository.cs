using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.Core.Helpers
{
    public static class Repository
    {
        private static readonly IMongoDatabase _db;

        static Repository()
        {
            MongoClient client = new MongoClient();
            _db = client.GetDatabase("WarcraftGuild");
        }

        public static async Task Insert<T>(T data) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            await collection.InsertOneAsync(data).ConfigureAwait(false);
        }

        public static async Task<List<T>> LoadAll<T>() where T: WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var result = await collection.FindAsync(new BsonDocument()).ConfigureAwait(false);
            return result.ToList();
        }

        public static async Task<T> LoadByGuid<T>(Guid id) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await collection.FindAsync(filter).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public static async Task<T> LoadByBlizzardId<T>(ulong blizzardId) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("BlizzardId", blizzardId);
            var result = await collection.FindAsync(filter).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public static async Task Delete<T>(T data) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            await DeleteByBlizzardId<T>(data.BlizzardId).ConfigureAwait(false);
        }

        public static async Task DeleteById<T>(Guid id) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.DeleteOneAsync(filter);
        }

        public static async Task DeleteByBlizzardId<T>(ulong blizzardId) where T : WoWData, new()
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("BlizzardId", blizzardId);
            await collection.DeleteOneAsync(filter);
        }
    }
}
