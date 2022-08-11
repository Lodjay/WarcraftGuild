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
        public TimeSpan DefaultAsyncDelay { get; private set; }
        public IOptions<BlizzardApiConfiguration> DefaultConfiguration { get { return Options.Create(DefaultConfig); } }

        public BlizzardApiReaderTests()
        {
            DefaultConfig = new BlizzardApiConfiguration
            {
                ApiRegion = Region.Europe,
                Locale = Locale.French,
                ClientId = "7cface7352224419a5678ba897d81af1",
                ClientSecret = "mJ7Wj6KnbfWugKdJ1PxiswBycvsUnrjh",
                Limiters = new List<Limiter>
                {
                    new Limiter{RatesPerTimespan = 36000, TimeBetweenLimitReset = new TimeSpan(1,0,0)},
                    new Limiter{RatesPerTimespan = 100, TimeBetweenLimitReset = new TimeSpan(0,0,1)},
                }
            };
            DefaultAsyncDelay = new TimeSpan(500);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Ctor_ShouldThrowsArgumentNullException()
        {
            WebClientMocker webClient = new();
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(null, webClient.WebClient));
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(DefaultConfiguration, null));
        }

    }
}
