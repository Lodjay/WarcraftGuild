using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class BlizzardApiReaderCheckTests : IClassFixture<BlizzardApiReaderTests>
    {
        private BlizzardApiReaderTests BlizzardApiReaderTests { get; set; }

        public BlizzardApiReaderCheckTests(BlizzardApiReaderTests blizzardApiReaderTests)
        {
            BlizzardApiReaderTests = blizzardApiReaderTests;
        }

        [Fact]
        public async Task Check_SuccessAuth()
        {
            WebClientMocker webClient = new WebClientMocker();
            webClient.SetupAuth(true);
            BlizzardApiReader api = new BlizzardApiReader(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);
            Exception ex = await Record.ExceptionAsync(() => api.Check()).ConfigureAwait(false);
            Assert.Null(ex);
            webClient.VerifyAuth(Times.Once());
        }

        [Fact]
        public async Task Check_BrokenLimit()
        {
            BlizzardApiConfiguration config = BlizzardApiReaderTests.DefaultConfig.Clone();
            config.Limiter = new List<Limiter>
            {
                new Limiter{RatesPerTimespan = 10, TimeBetweenLimitReset = new TimeSpan(0,0,5)},
            };
            WebClientMocker webClient = new WebClientMocker();
            webClient.SetupAuth(true);
            BlizzardApiReader api = new BlizzardApiReader(Options.Create(config), webClient.WebClient);

            List<Exception> exceptions = new List<Exception>();
            int count = config.Limiter.First().RatesPerTimespan;
            for (int i = 0; i < count + 1; i++)
            {
                Exception ex = await Record.ExceptionAsync(() => api.Check()).ConfigureAwait(false);
                if (ex != null)
                    exceptions.Add(ex);
            }
            Assert.Single(exceptions);
            Assert.IsType<RateLimitReachedException>(exceptions.First());
            webClient.VerifyAuth(Times.Once());
        }

        [Fact]
        public async Task Check_FailAuth()
        {
            WebClientMocker webClient = new WebClientMocker();
            webClient.SetupAuth(false);
            BlizzardApiReader api = new BlizzardApiReader(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);
            await Assert.ThrowsAsync<HttpRequestException>(() => api.Check());
            webClient.VerifyAuth(Times.Once());
        }
    }
}
