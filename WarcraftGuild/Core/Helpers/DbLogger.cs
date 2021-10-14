using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Configuration;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;
using System.Text;

namespace WarcraftGuild.WoW.Handlers
{

    public static class DbLogger
    {
        private static readonly IMongoDatabase _db;
        static DbLogger()
        {
            MongoClientSettings settings = new MongoClientSettings
            {
                MinConnectionPoolSize = 100,
                MaxConnectionPoolSize = 500
            };
            MongoClient client = new MongoClient(settings);
            _db = client.GetDatabase("Logs");
        }

        public  static void Log(string message, LogLevel logLevel = LogLevel.Debug)
        {
            LogEvent log = new LogEvent
            {
                Message = message,
                Severity = logLevel,
                LogDate = DateTime.Now
            };
            Task.Run(() => LogInDb(log));
        }

        public static void LogExeption(Exception ex, LogLevel logLevel = LogLevel.Critical)
        {
            LogEvent log = new LogEvent
            {
                Message = ex.Message,
                Trace = ex.StackTrace,
                Severity = logLevel,
                LogDate = DateTime.Now
            };
            Task.Run(() => LogInDb(log));
            if (ex.InnerException != null)
                LogExeption(ex.InnerException, logLevel);
        }

        private async static Task LogInDb(LogEvent log)
        {
            var collection = _db.GetCollection<LogEvent>(typeof(LogEvent).Name);
            await collection.InsertOneAsync(log).ConfigureAwait(false);
        }
    }
}