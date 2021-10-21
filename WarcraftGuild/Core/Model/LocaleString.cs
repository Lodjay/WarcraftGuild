using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.Core.Models
{
    public class LocaleString
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Code { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<Locale, string> Values { get; set; }

        public string GetLocaleValue(Locale locale)
        {
            return Values.ContainsKey(locale) ? Values[locale] : Code;
        }
    }
}