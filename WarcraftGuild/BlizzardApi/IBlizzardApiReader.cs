using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.BlizzardApi
{
    public interface IBlizzardApiReader
    {
        Task<T> GetAsync<T>(string query, Namespace? ns = null, string additionalParams = null);

        Task<string> GetJsonAsync(string query, Namespace? ns = null, string additionalParams = null);

        Task<IApiResponse> Authentificate();

        Task<string> SendTokenRequest();
    }
}