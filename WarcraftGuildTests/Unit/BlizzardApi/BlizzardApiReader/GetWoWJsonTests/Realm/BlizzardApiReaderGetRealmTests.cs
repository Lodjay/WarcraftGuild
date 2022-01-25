﻿using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WarcraftGuild.Domain.Core.Enums;
using WarcraftGuild.Domain.Core.Exceptions;
using WarcraftGuild.Domain.Core.Extensions;
using WarcraftGuild.Domain.Core.Json;
using WarcraftGuild.Infrastructure.BlizzardApi;
using WarcraftGuild.Infrastructure.BlizzardApi.Configuration;
using WarcraftGuild.Tests.DataGenerator.WoWJson;
using WarcraftGuild.Tests.Unit.BlizzardApi.Helpers;
using Xunit;

namespace WarcraftGuild.Tests.Unit.BlizzardApi.GetWoWJsonTests.Realm
{
    public class BlizzardApiReaderGetRealmTests : IClassFixture<BlizzardApiReaderTests>
    {
        private BlizzardApiReaderTests BlizzardApiReaderTests { get; set; }
        private RealmJson ExpectedJson { get; set; }

        public BlizzardApiReaderGetRealmTests(BlizzardApiReaderTests blizzardApiReaderTests)
        {
            BlizzardApiReaderTests = blizzardApiReaderTests;
            ExpectedJson = WoWJsonGenerator.RandomRealmJson();
        }

        [Fact]
        public async Task GetJson_Valid()
        {
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.OK, JsonSerializer.Serialize(ExpectedJson));
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);

            RealmJson jsonResult = await api.GetAsync<RealmJson>("test", Namespace.Static).ConfigureAwait(false);
            Assert.True(ExpectedJson.IsClone(jsonResult));
            webClient.VerifyAuth(Times.Once());
            webClient.VerifyApiRequest(It.IsAny<string>(), Times.Once());
        }

        [Fact]
        public async Task GetJson_BreakLimit()
        {
            BlizzardApiConfiguration config = BlizzardApiReaderTests.DefaultConfig.Clone();
            config.Limiter = new List<Limiter>
            {
                new Limiter{RatesPerTimespan = 10, TimeBetweenLimitReset = new TimeSpan(0,0,5)},
            };
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.OK, JsonSerializer.Serialize(ExpectedJson));
            BlizzardApiReader api = new(Options.Create(config), webClient.WebClient);

            List<Exception> exceptions = new();
            int count = config.Limiter.First().RatesPerTimespan;
            for (int i = 0; i < count + 1; i++)
            {
                Exception ex = await Record.ExceptionAsync(() => api.GetAsync<RealmJson>("test", Namespace.Static)).ConfigureAwait(false);
                if (ex != null)
                    exceptions.Add(ex);
            }
            Assert.Single(exceptions);
            Assert.IsType<RateLimitReachedException>(exceptions.First());
            webClient.VerifyAuth(Times.Once());
            webClient.VerifyApiRequest(It.IsAny<string>(), Times.Exactly(count));
        }

        [Fact]
        public async Task GetJson_ApiFailed()
        {
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.InternalServerError, JsonSerializer.Serialize(ExpectedJson));
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);

            await Assert.ThrowsAsync<BadResponseException>(() => api.GetAsync<RealmJson>("test", Namespace.Static)).ConfigureAwait(false);
            webClient.VerifyAuth(Times.Once());
            webClient.VerifyApiRequest(It.IsAny<string>(), Times.Once());
        }
    }
}