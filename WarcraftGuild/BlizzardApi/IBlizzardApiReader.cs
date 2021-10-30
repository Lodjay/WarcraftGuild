using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.BlizzardApi
{
    public interface IBlizzardApiReader
    {
        Task<WoWJson> GetAsync<WoWJson>(string query, Namespace? ns = null) where WoWJson : BlizzardApiJsonResponse, new();

        Task<string> GetJsonAsync(string query, Namespace? ns = null);

        Task<IApiResponse> Authentificate();

        Task<string> SendTokenRequest();
    }
}