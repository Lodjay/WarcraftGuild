using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Exceptions;
using WarcraftGuild.Core.Extensions;
using WarcraftGuildTests.Unit.BlizzardApi.Helpers;
using Xunit;

namespace WarcraftGuildTests.Unit.BlizzardApi
{
    public class BlizzardApiReaderGetJsonTests : IClassFixture<BlizzardApiReaderTests>
    {
        private BlizzardApiReaderTests BlizzardApiReaderTests { get; set; }
        private string ExpectedJson { get; set; }

        public BlizzardApiReaderGetJsonTests(BlizzardApiReaderTests blizzardApiReaderTests)
        {
            BlizzardApiReaderTests = blizzardApiReaderTests;
            ExpectedJson = "Test";
        }

        [Fact]
        public async Task GetJson_Valid()
        {
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.OK, ExpectedJson);
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);

            string jsonResult = await api.GetJsonAsync("test").ConfigureAwait(false);
            Assert.Equal(ExpectedJson, jsonResult);
            webClient.VerifyAuth(Times.Once());
            webClient.VerifyApiRequest(It.IsAny<string>(), Times.Once());
        }

        [Fact]
        public async Task GetJson_BreakLimit()
        {
            BlizzardApiConfiguration config = BlizzardApiReaderTests.DefaultConfig.Clone();
            config.Limiters = new List<Limiter>
            {
                new Limiter{RatesPerTimespan = 10, TimeBetweenLimitReset = new TimeSpan(0,0,5)},
            };
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.OK, ExpectedJson);
            BlizzardApiReader api = new(Options.Create(config), webClient.WebClient);

            List<Exception> exceptions = new();
            int count = config.Limiters.First().RatesPerTimespan;
            for (int i = 0; i < count + 1; i++)
            {
                Exception ex = await Record.ExceptionAsync(() => api.GetJsonAsync("test")).ConfigureAwait(false);
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
            webClient.SetupApiRequest(It.IsAny<string>(), HttpStatusCode.InternalServerError, ExpectedJson);
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);

            await Assert.ThrowsAsync<BadResponseException>(() => api.GetJsonAsync("test")).ConfigureAwait(false);
            webClient.VerifyAuth(Times.Once());
            webClient.VerifyApiRequest(It.IsAny<string>(), Times.Once());
        }
    }
}
