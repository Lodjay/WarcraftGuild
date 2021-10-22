using Moq;
using System.Net;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Exceptions;
using Xunit;

namespace WarcraftGuildTests.Unit.BlizzardApi
{
    public class BlizzardApiReaderGetJsonTests : IClassFixture<BlizzardApiReaderTests>
    {
        private BlizzardApiReaderTests _blizzardApiReaderTests;
        private string AssertedJson { get; set; }

        public BlizzardApiReaderGetJsonTests(BlizzardApiReaderTests blizzardApiReaderTests)
        {
            _blizzardApiReaderTests = blizzardApiReaderTests;
            AssertedJson = "Test";
        }

        [Fact]
        public async void GetJson_Valid()
        {
            Mock<IApiResponse> Response = new Mock<IApiResponse>();
            Response.Setup(x => x.GetStatusCode()).Returns(HttpStatusCode.OK);
            Response.Setup(x => x.ReadContentAsync()).ReturnsAsync(AssertedJson, _blizzardApiReaderTests.AsyncDelay);
            _blizzardApiReaderTests.WebClient.Setup(x => x.MakeApiRequestAsync(It.IsAny<string>())).ReturnsAsync(Response.Object, _blizzardApiReaderTests.AsyncDelay);

            string jsonResult = await _blizzardApiReaderTests.BlizzardApiReader.GetJsonAsync("test", Namespace.Static).ConfigureAwait(false);
            Assert.Equal(AssertedJson, jsonResult);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetJson_ApiFailed()
        {
            Mock<IApiResponse> Response = new Mock<IApiResponse>();
            Response.Setup(x => x.GetStatusCode()).Returns(HttpStatusCode.BadRequest);
            Response.Setup(x => x.ReadContentAsync()).ReturnsAsync(AssertedJson);
            _blizzardApiReaderTests.WebClient.Setup(x => x.MakeApiRequestAsync(It.IsAny<string>())).ReturnsAsync(Response.Object);

            await Assert.ThrowsAsync<BadResponseException>(() => _blizzardApiReaderTests.BlizzardApiReader.GetJsonAsync("test", Namespace.Static)).ConfigureAwait(false);

        }
    }
}
