using Moq;
using System;
using System.Net;
using System.Text.Json;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuildTests.Unit.BlizzardApi.Helpers
{
    public class IApiReponseMocker
    {
        public Mock<IApiResponse> Mock { get; private set; }
        public IApiResponse Response { get { return Mock.Object; } }
        public TimeSpan AsyncDelay { get; private set; }

        public IApiReponseMocker()
        {
            Mock = new Mock<IApiResponse>();
            AsyncDelay = new TimeSpan(50);
        }

        public void Setup(HttpStatusCode code, string jsonContent, TimeSpan? time = null)
        {
            Mock.Setup(x => x.GetStatusCode()).Returns(code);
            Mock.Setup(x => x.ReadContentAsync()).ReturnsAsync(jsonContent, time ?? AsyncDelay); ;
        }
    }
}
