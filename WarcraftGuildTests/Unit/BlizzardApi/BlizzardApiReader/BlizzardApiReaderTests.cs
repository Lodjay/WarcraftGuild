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
using Xunit;

namespace WarcraftGuildTests.Unit.BlizzardApi
{
    public class BlizzardApiReaderTests : IDisposable
    {
        public IOptions<BlizzardApiConfiguration> Config { get; private set; }
        public Mock<IWebClient> WebClient { get; private set; }
        public BlizzardApiReader BlizzardApiReader { get; private set; }
        public TimeSpan AsyncDelay { get; private set; }

        public BlizzardApiReaderTests()
        {
            Config = Options.Create(new BlizzardApiConfiguration
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
            });
            AsyncDelay = new TimeSpan(500);
            MockDefaults();
            BlizzardApiReader = new BlizzardApiReader(Config, WebClient.Object);
        }

        private void MockDefaults()
        {
            WebClient = new Mock<IWebClient>();
            Mock<IApiResponse> Response = new Mock<IApiResponse>();
            Response.Setup(x => x.GetStatusCode()).Returns(HttpStatusCode.OK);
            Response.Setup(x => x.ReadContentAsync()).ReturnsAsync("{\"access_token\":\"EUDMYmwlmK3JM6ZKf54hHhNSRxd0IMxFNL\",\"token_type\":\"bearer\",\"expires_in\":86399,\"sub\":\"7cface7352224419a5678ba897d81af1\"}", AsyncDelay);
            WebClient.Setup(x => x.RequestAccessTokenAsync()).ReturnsAsync(Response.Object);
        }

        public void Dispose()
        {

        }

        [Fact]
        public void IncorrectCtorShouldThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(null, WebClient.Object));
            Assert.Throws<ArgumentNullException>(() => new BlizzardApiReader(Config, null));
        }

    }
}
