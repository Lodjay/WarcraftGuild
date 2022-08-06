using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Models
{
    public class ApiResponse : IApiResponse
    {
        private readonly HttpResponseMessage response;

        public ApiResponse(HttpResponseMessage responseMessage)
        {
            response = responseMessage;
        }

        public HttpStatusCode GetStatusCode()
        {
            return response.StatusCode;
        }

        public async Task<string> ReadContentAsync()
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}