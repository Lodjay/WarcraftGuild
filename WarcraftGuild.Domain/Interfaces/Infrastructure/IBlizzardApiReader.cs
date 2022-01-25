using WarcraftGuild.Domain.Core.Enums;
using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.Interfaces.Infrastructure
{
    public interface IBlizzardApiReader
    {
        Task<IApiResponse> Authentificate();

        Task Check();

        Task<WoWJson> GetAsync<WoWJson>(string query, Namespace? ns = null) where WoWJson : BlizzardApiJsonResponse, new();

        Task<string> GetJsonAsync(string query, Namespace? ns = null);

        Task<string> SendTokenRequest();
    }
}