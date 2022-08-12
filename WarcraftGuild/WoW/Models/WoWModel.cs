using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Helpers;

namespace WarcraftGuild.WoW.Models
{
    public abstract class WoWModel
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.Int64, AllowOverflow = true)]
        public ulong BlizzardId { get; set; }
        public string BlizzardApiComment { get; set; }
        public DateTime UpdateDate { get; set; }

        private void Load<TJson>(TJson json) where TJson : BlizzardApiJsonResponse
        {
            switch (json.ResultCode)
            {
                case null:
                case System.Net.HttpStatusCode.OK:
                    break;

                case System.Net.HttpStatusCode.Forbidden:
                    BlizzardApiComment = LocaleStringCode.BLIZZ_API_RESPONSE_FORBIDDEN;
                    break;

                case System.Net.HttpStatusCode.NotFound:
                    BlizzardApiComment = LocaleStringCode.BLIZZ_API_RESPONSE_NOT_FOUND;
                    break;

                case System.Net.HttpStatusCode.TooManyRequests:
                    BlizzardApiComment = LocaleStringCode.BLIZZ_API_LIMIT_REACHED;
                    break;

                default:
                    BlizzardApiComment = LocaleStringCode.UNKNOWN_ERROR;
                    break;
            }
        }

        protected bool CheckJson<TJson>(TJson json) where TJson : BlizzardApiJsonResponse
        {
            if (json == null)
            {
                BlizzardApiComment = LocaleStringCode.BLIZZ_API_RESPONSE_EMPTY;
                return false;
            }
            Load(json);
            return true;
        }
    }
}