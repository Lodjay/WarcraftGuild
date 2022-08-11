using System;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.BlizzardApi
{
    public interface IBlizzardApiReader
    {
        Task<WoWJson> GetAsync<WoWJson>(string query, Namespace? ns = null, bool force = false) where WoWJson : BlizzardApiJsonResponse, new();

        Task<string> GetJsonAsync(string query, Namespace? ns = null, bool force = false);

        Limiter GetShorterLimiter();

        Task<IApiResponse> Authentificate();

        Task<string> SendTokenRequest();
    }
}