using System.Net;

namespace WarcraftGuild.Domain.Interfaces
{
    public interface IApiResponse
    {
        HttpStatusCode GetStatusCode();

        Task<string> ReadContentAsync();
    }
}