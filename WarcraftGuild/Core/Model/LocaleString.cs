using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.Core.Models
{
    public class LocaleString
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Dictionary<Locale, string> Values { get; set; }
    }
}
