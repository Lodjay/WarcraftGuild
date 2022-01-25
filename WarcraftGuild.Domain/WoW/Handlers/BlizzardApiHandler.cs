using Microsoft.Extensions.Options;
using WarcraftGuild.Domain.Core.Enums;
using WarcraftGuild.Domain.Core.Json;
using WarcraftGuild.Domain.Interfaces;
using WarcraftGuild.Domain.Interfaces.Infrastructure;
using WarcraftGuild.Domain.WoW.Configuration;
using WarcraftGuild.Domain.WoW.Models;

namespace WarcraftGuild.Domain.WoW.Handlers
{
    public class BlizzardApiHandler : IBlizzardApiHandler
    {
        private readonly ApiConfiguration _config;
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly IDbManager _dbManager;

        public BlizzardApiHandler(IOptions<ApiConfiguration> apiConfiguration, IBlizzardApiReader blizzardApiReader, IDbManager dbManager)
        {
            _config = apiConfiguration.Value ?? throw new ArgumentNullException(nameof(apiConfiguration));
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }

        public async Task<ConnectedRealm> GetConnectedRealmById(ulong blizzardId, bool forceUpdate = false)
        {
            ConnectedRealm connectedRealm = forceUpdate ? null : await GetFromDbByBlizzardId<ConnectedRealm>(blizzardId).ConfigureAwait(false);
            bool update = forceUpdate || await CheckDbData(connectedRealm).ConfigureAwait(false);
            if (update)
                connectedRealm = await DbInsertFromApi<ConnectedRealm, ConnectedRealmJson>($"data/wow/connected-realm/{blizzardId}", Namespace.Dynamic).ConfigureAwait(false);
            return connectedRealm;
        }

        public async Task<Realm> GetRealmBySlug(string slug, bool forceUpdate = false)
        {
            Realm realm = forceUpdate ? null : await _dbManager.GetRealmBySlug(slug).ConfigureAwait(false);
            bool update = forceUpdate || await CheckDbData(realm).ConfigureAwait(false);
            if (update)
                realm = await DbInsertFromApi<Realm, RealmJson>($"data/wow/realm/{slug}", Namespace.Dynamic).ConfigureAwait(false);
            return realm;
        }

        public async Task<Guild> GetGuildBySlug(string realmSlug, string guildSlug, bool forceUpdate = false)
        {
            Realm realm = await GetRealmBySlug(realmSlug, forceUpdate).ConfigureAwait(false);
            Guild guild = forceUpdate ? null : await _dbManager.GetGuildBySlug(realm.Slug, guildSlug).ConfigureAwait(false);
            bool update = forceUpdate || await CheckDbData(guild).ConfigureAwait(false);
            if (update)
            {
                guild = await DbInsertFromApi<Guild, GuildJson>($"data/wow/guild/{realm.Slug}/{guildSlug}", Namespace.Dynamic).ConfigureAwait(false);
                guild.Slug = guildSlug;
                guild.Load(await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{guild}/{guildSlug}/achievements", Namespace.Profile).ConfigureAwait(false));
                guild.Load(await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{guild}/{guildSlug}/roster ", Namespace.Profile).ConfigureAwait(false));
            }
            return guild;
        }

        private async Task<TModel> GetFromDbByBlizzardId<TModel>(ulong blizzardId) where TModel : WoWModel, new()
        {
            TModel model = await _dbManager.GetByBlizzardId<TModel>(blizzardId).ConfigureAwait(false);
            return model;
        }

        private async Task<bool> CheckDbData<TModel>(TModel model) where TModel : WoWModel, new()
        {
            if (model == null)
                return true;
            if (model.UpdateDate.AddDays(_config.DataExpiryDays) > DateTime.Now)
            {
                await _dbManager.Delete(model).ConfigureAwait(false);
                return true;
            }
            return false;
        }

        private async Task<TModel> DbInsertFromApi<TModel, TJson>(string query, Namespace? ns = null) where TModel : WoWModel, new() where TJson : BlizzardApiJsonResponse, new()
        {
            TJson json = await _blizzardApiReader.GetAsync<TJson>(query, ns).ConfigureAwait(false);
            TModel model = new();
            model.Load(json);
            await _dbManager.Insert(model).ConfigureAwait(false);
            return model;
        }
    }
}