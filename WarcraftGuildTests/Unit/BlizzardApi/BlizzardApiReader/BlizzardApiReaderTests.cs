using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.Core.Enums;
using WarcraftGuildTests.Unit.BlizzardApi.Helpers;
using Xunit;

namespace WarcraftGuildTests.Unit.BlizzardApi
{
    public class BlizzardApiReaderTests : IDisposable
    {
        public BlizzardApiConfiguration DefaultConfig { get; private set; }
        public WebClientMocker DefaultWebClientMocker { get; private set; }
        public TimeSpan DefaultAsyncDelay { get; private set; }
        public IOptions<BlizzardApiConfiguration> DefaultConfiguration { get { return Options.Create(DefaultConfig); } }
        public IWebClient DefaultWebClient { get { return DefaultWebClientMocker.WebClient; } }

        public BlizzardApiReaderTests()
        {
            DefaultConfig = new BlizzardApiConfiguration
            {
                ApiRegion = Region.Europe,
                Locale = Locale.French,
                ClientId = "7cface7352224419a5678ba897d81af1",
                ClientSecret = "mJ7Wj6KnbfWugKdJ1PxiswBycvsUnrjh",
                Limiter = new List<Limiter>
                {
                    new Limiter{RatesPerTimespan = 36000, TimeBetweenLimitReset = new TimeSpan(1,0,0)},
                    new Limiter{RatesPerTimespan = 100, TimeBetweenLimitReset = new TimeSpan(0,0,1)},
                }
            };
            DefaultWebClientMocker = new WebClientMocker();
            DefaultWebClientMocker.SetupAuth(true);
            DefaultAsyncDelay = new TimeSpan(500);
        }


        public void Dispose()
        {

        }

        [Fact]
        public void Ctor_ShouldThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(null, DefaultWebClient));
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(DefaultConfiguration, null));
        }

        [Fact]
        public async Task Check_SuccessAuth()
        {
            BlizzardApiReader api = new BlizzardApiReader(DefaultConfiguration, DefaultWebClient);
            Exception ex = await Record.ExceptionAsync(() => api.Check()).ConfigureAwait(false);
            Assert.Null(ex);
        }

        [Fact]
        public async Task Checkdouble_SuccessAuth()
        {
            BlizzardApiReader api = new BlizzardApiReader(DefaultConfiguration, DefaultWebClient);
            Exception ex = await Record.ExceptionAsync(() => api.Check()).ConfigureAwait(false);
            Assert.Null(ex);
            ex = await Record.ExceptionAsync(() => api.Check()).ConfigureAwait(false);
            Assert.Null(ex);
        }

        [Fact]
        public async Task Check_FailAuth()
        {
            WebClientMocker webClient = DefaultWebClientMocker;
            webClient.SetupAuth(false);
            BlizzardApiReader api = new BlizzardApiReader(DefaultConfiguration, webClient.WebClient);
            await Assert.ThrowsAsync<HttpRequestException>(() => api.Check());
        }

    }
}
