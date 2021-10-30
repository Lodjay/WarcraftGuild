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
        public ulong BlizzardId { get; set; }
        public string BlizzardApiComment { get; set; }
        public DateTime UpdateDate { get; set; }

        public void Load(BlizzardApiJsonResponse json)
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

        protected bool CheckJson(BlizzardApiJsonResponse json)
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