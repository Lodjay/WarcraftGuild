using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WarcraftGuild.Domain.Core.Exceptions;
using WarcraftGuild.Domain.Core.Extensions;
using WarcraftGuild.Infrastructure.BlizzardApi;
using WarcraftGuild.Infrastructure.BlizzardApi.Configuration;
using WarcraftGuild.Tests.Unit.BlizzardApi.Helpers;
using Xunit;

namespace WarcraftGuild.Tests.Unit.BlizzardApi
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
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);
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
            WebClientMocker webClient = new();
            webClient.SetupAuth(true);
            BlizzardApiReader api = new(Options.Create(config), webClient.WebClient);

            List<Exception> exceptions = new();
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
            WebClientMocker webClient = new();
            webClient.SetupAuth(false);
            BlizzardApiReader api = new(BlizzardApiReaderTests.DefaultConfiguration, webClient.WebClient);
            await Assert.ThrowsAsync<HttpRequestException>(() => api.Check());
            webClient.VerifyAuth(Times.Once());
        }
    }
}