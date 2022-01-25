using System.Net;
using WarcraftGuild.Domain.Interfaces;

namespace WarcraftGuild.Infrastructure.BlizzardApi.Models
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