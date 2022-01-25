using Moq;
using System;
using System.Net;
using WarcraftGuild.Domain.Interfaces;

namespace WarcraftGuild.Tests.Unit.BlizzardApi.Helpers
{
    public class WebClientMocker
    {
        public Mock<IWebClient> Mock { get; private set; }

        public IWebClient WebClient
        { get { return Mock.Object; } }
        public TimeSpan AsyncDelay { get; private set; }

        public WebClientMocker()
        {
            Mock = new Mock<IWebClient>();
            AsyncDelay = new TimeSpan(50);
        }

        public void SetupAuth(bool valid, TimeSpan? time = null)
        {
            IApiReponseMocker response = new();
            if (valid)
            {
                response.Setup(HttpStatusCode.OK, "{\"access_token\":\"EUDMYmwlmK3JM6ZKf54hHhNSRxd0IMxFNL\",\"token_type\":\"bearer\",\"expires_in\":86399,\"sub\":\"7cface7352224419a5678ba897d81af1\"}", time ?? AsyncDelay);
            }
            else
            {
                response.Setup(HttpStatusCode.Forbidden, "{\"AnyError\":\"Ooops\",\"token_type\":\"crash\",\"expires_in\":0,\"sub\":\"0\"}", time ?? AsyncDelay);
            }
            Mock.Setup(x => x.RequestAccessTokenAsync()).ReturnsAsync(response.Response, time ?? AsyncDelay);
        }

        public void SetupApiRequest(string path, HttpStatusCode code, string expectedJson, TimeSpan? time = null)
        {
            IApiReponseMocker response = new();
            response.Setup(code, expectedJson, time);
            if (string.IsNullOrEmpty(path))
                Mock.Setup(x => x.MakeApiRequestAsync(It.IsAny<string>())).ReturnsAsync(response.Response, time ?? AsyncDelay);
            else
                Mock.Setup(x => x.MakeApiRequestAsync(It.Is<string>(s => s == path))).ReturnsAsync(response.Response, time ?? AsyncDelay);
        }

        public void VerifyAuth(Times times)
        {
            Mock.Verify(x => x.RequestAccessTokenAsync(), times);
        }

        public void VerifyApiRequest(string path, Times times)
        {
            if (string.IsNullOrEmpty(path))
                Mock.Verify(x => x.MakeApiRequestAsync(It.IsAny<string>()), times);
            else
                Mock.Verify(x => x.MakeApiRequestAsync(It.Is<string>(s => s == path)), times);
        }
    }
}