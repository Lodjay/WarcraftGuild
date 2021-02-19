using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Models
{
    public class ApiResponse : IApiResponse
    {
        readonly HttpResponseMessage response;

        public ApiResponse(HttpResponseMessage responseMessage)
        {
            response = responseMessage;
        }

        public HttpStatusCode StatusCode()
        {
            return response.StatusCode;
        }

        public bool IsSuccessful()
        {
            return response.IsSuccessStatusCode;
        }

        public async Task<string> ReadContentAsync()
        {
            return await response.Content.ReadAsStringAsync();
        }

    }
}
