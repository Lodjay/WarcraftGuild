using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Interfaces
{
    public interface IWebClient
    {
        Task<IApiResponse> MakeApiRequestAsync(string path);
        Task<IApiResponse> RequestAccessTokenAsync();
        Task<IApiResponse> RequestUserAuthorizeAsync();
    }
}
