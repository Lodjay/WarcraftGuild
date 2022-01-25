using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Attributes;

namespace WarcraftGuild.Domain.Core.Model
{
    public class LogEvent
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Message { get; set; }
        public string Description { get; set; }
        public string Collection { get; set; }
        public ulong BlizzardId { get; set; }
        public string KeyData { get; set; }
        public LogLevel Severity { get; set; }
        public DateTime LogDate { get; set; }
    }
}