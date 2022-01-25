using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using WarcraftGuild.Domain.Core.Enums;

namespace WarcraftGuild.Domain.Core.Model
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