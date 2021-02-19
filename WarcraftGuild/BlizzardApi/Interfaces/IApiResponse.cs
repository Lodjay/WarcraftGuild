using System.Net;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Interfaces
{
    public interface IApiResponse
    {
        HttpStatusCode StatusCode();
        bool IsSuccessful();
        Task<string> ReadContentAsync();
    }
}
