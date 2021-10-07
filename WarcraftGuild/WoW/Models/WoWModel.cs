using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        protected bool CheckJson(WoWJson json)
        {
            if (json == null)
            {
                BlizzardApiComment = Messages.NOT_FOUND;
                return false;
            }
            switch (json.ResultCode)
            {
                case null:
                case System.Net.HttpStatusCode.OK:
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    BlizzardApiComment = Messages.FORBIDDEN;
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    BlizzardApiComment = Messages.NOT_FOUND;
                    break;
                default:
                    BlizzardApiComment = Messages.UNKNOWN_ERROR;
                    break;
            }
            return true;
        }
    }
}
