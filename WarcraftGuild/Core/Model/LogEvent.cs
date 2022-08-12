using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WarcraftGuild.Core.Handlers
{
    public class LogEvent
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string Collection { get; set; }

        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong BlizzardId { get; set; }
        public string KeyData { get; set; }
        public LogLevel Severity { get; set; }
        public DateTime LogDate { get; set; }
    }
}