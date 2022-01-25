using System;
using System.Collections.Generic;
using System.Net;
using WarcraftGuild.Domain.Core.Enums;
using WarcraftGuild.Domain.Core.Extensions;
using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Tests.DataGenerator.WoWJson
{
    public static class WoWJsonGenerator
    {
        public static List<BlizzardApiJsonResponse> GenerateAllWoWJson()
        {
            List<BlizzardApiJsonResponse> WoWJsonList = new();
            WoWJsonList.AddRange(GenerateRealmJsons());
            return WoWJsonList;
        }

        public static List<BlizzardApiJsonResponse> GenerateRealmJsons()
        {
            List<RealmJson> realms = RandomRealmJsonList();
            return new List<BlizzardApiJsonResponse>
            {
                RandomConnectedRealmIndexJson(),
                RandomConnectedRealmJson(realms),
                RandomRealmIndexJson(realms),
                RandomRealmJson()
            };
        }

        #region Common

        public static TypeJson RandomTypeJson(string forceType)
        {
            return new TypeJson
            {
                Id = RandomDataGenerator.RandomUint(),
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
                Name = forceType,
                Type = forceType.ToUpper(),
            };
        }

        public static TypeJson RandomTypeJson()
        {
            return RandomTypeJson(RandomDataGenerator.RandomName(10));
        }

        public static HrefJson RandomHrefJson()
        {
            return new HrefJson
            {
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
                Uri = new Uri(RandomDataGenerator.RandomName(10))
            };
        }

        public static List<HrefJson> RandomHrefJsonList()
        {
            List<HrefJson> hrefJsonList = new();
            for (int i = 0; i < RandomDataGenerator.RandomUint(10); i++)
                hrefJsonList.Add(RandomHrefJson());
            return hrefJsonList;
        }

        #endregion Common

        #region Realm

        public static List<RealmJson> RandomRealmJsonList()
        {
            List<RealmJson> realms = new();
            for (int i = 0; i < RandomDataGenerator.RandomUint(10); i++)
                realms.Add(RandomRealmJson());
            return realms;
        }

        public static RealmJson RandomRealmJson(string forceName)
        {
            return new RealmJson
            {
                Id = RandomDataGenerator.RandomUint(),
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
                Category = RandomDataGenerator.RandomName(10),
                Locale = RandomDataGenerator.RandomEnum<Locale>().GetCode(),
                Name = forceName,
                Slug = forceName.Slugify(),
                Timezone = RandomDataGenerator.RandomName(5),
                Tournament = RandomDataGenerator.RandomBool(),
                Type = RandomTypeJson()
            };
        }

        public static RealmJson RandomRealmJson()
        {
            return RandomRealmJson(RandomDataGenerator.RandomName(15));
        }

        public static RealmIndexJson RandomRealmIndexJson(List<RealmJson> forceRealms)
        {
            return new RealmIndexJson()
            {
                Realms = forceRealms,
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
            };
        }

        public static RealmIndexJson RandomRealmIndexJson()
        {
            return RandomRealmIndexJson(RandomRealmJsonList());
        }

        public static ConnectedRealmJson RandomConnectedRealmJson(List<RealmJson> forceRealms)
        {
            return new ConnectedRealmJson
            {
                Id = RandomDataGenerator.RandomUint(),
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
                HasQueue = RandomDataGenerator.RandomBool(),
                Population = RandomTypeJson(),
                Status = RandomTypeJson(),
                Realms = forceRealms
            };
        }

        public static ConnectedRealmJson RandomConnectedRealmJson()
        {
            return RandomConnectedRealmJson(RandomRealmJsonList());
        }

        public static ConnectedRealmIndexJson RandomConnectedRealmIndexJson(List<HrefJson> forceList)
        {
            return new ConnectedRealmIndexJson()
            {
                ConnectedRealms = forceList,
                DirectlyCalled = true,
                ResultCode = HttpStatusCode.OK,
            };
        }

        public static ConnectedRealmIndexJson RandomConnectedRealmIndexJson()
        {
            return RandomConnectedRealmIndexJson(RandomHrefJsonList());
        }

        #endregion Realm
    }
}