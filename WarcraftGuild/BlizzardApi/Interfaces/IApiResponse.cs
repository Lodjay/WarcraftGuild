using System.Net;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Interfaces
{
    public interface IApiResponse
    {
        HttpStatusCode GetStatusCode();
        Task<string> ReadContentAsync();
    }
}
