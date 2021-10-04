using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Helpers;

namespace WarcraftGuild.WoW.Models
{
    public abstract class WoWData
    {
        [BsonId]
        public Guid Id { get; set; }
        public ulong BlizzardId { get; set; }
        public string BlizzardApiComment { get; set; }

        protected bool CanLoadJson<WoWJson>(WoWJson json) where WoWJson : BlizzardJson, new()
        {
            if (json == null)
                return false;
            switch (json.ResultCode)
            {
                case null:
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.Forbidden:
                    BlizzardApiComment = Messages.FORBIDDEN;
                    return false;
                case System.Net.HttpStatusCode.NotFound:
                    BlizzardApiComment = Messages.NOT_FOUND;
                    return false;
                default:
                    BlizzardApiComment = Messages.UNKNOWN_ERROR;
                    return false;
            }
        }
    }
}
