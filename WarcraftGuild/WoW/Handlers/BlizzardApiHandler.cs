using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;
using WarcraftGuild.WoW.Configuration;
using Microsoft.Extensions.Options;

namespace WarcraftGuild.WoW.Handlers
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
            ConnectedRealm connectedRealm = forceUpdate? null : await GetFromDbByBlizzardId<ConnectedRealm>(blizzardId).ConfigureAwait(false);
            bool update = forceUpdate || await CheckDbData(connectedRealm).ConfigureAwait(false);
            if (update)
                connectedRealm = await DbInsertFromApi<ConnectedRealm, ConnectedRealmJson>($"data/wow/connected-realm/{blizzardId}", Namespace.Dynamic).ConfigureAwait(false);
            return connectedRealm;
        }

        public async Task<Realm> GetRealmBySlug(string slug, bool forceUpdate = false)
        {
            Realm realm = forceUpdate ? null : await _dbManager.GetRealmBySlug(slug).ConfigureAwait(false) ;
            bool update = forceUpdate || await CheckDbData(realm).ConfigureAwait(false);
            if (update)
                realm = await DbInsertFromApi<Realm, RealmJson>($"data/wow/realm/{slug}", Namespace.Dynamic).ConfigureAwait(false);
            return realm;
        }

        public async Task<Guild> GetGuildByTag(string realmSlug, string guildTag, bool forceUpdate = false)
        {
            Realm realm = await GetRealmBySlug(realmSlug, forceUpdate).ConfigureAwait(false);
            Guild guild = forceUpdate ? null : await _dbManager.GetGuildByTag(realm.Slug, guildTag).ConfigureAwait(false);
            bool update = forceUpdate || await CheckDbData(guild).ConfigureAwait(false);
            if (update)
            {
                guild = new Guild();
                guild.Load(await _blizzardApiReader.GetAsync<GuildJson>($"data/wow/guild/{realm.Slug}/{guildTag}/ ", Namespace.Profile).ConfigureAwait(false));
                guild.RealmSlug = realmSlug;
                guild.Tag = guildTag;
                guild.LoadAchievements(await _blizzardApiReader.GetAsync<GuildAchievementsJson>($"data/wow/guild/{realm.Slug}/{guildTag}/achievements", Namespace.Profile).ConfigureAwait(false));
                guild.LoadRoster(await _blizzardApiReader.GetAsync<GuildRosterJson>($"data/wow/guild/{realm.Slug}/{guildTag}/roster ", Namespace.Profile).ConfigureAwait(false));

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
            DateTime expirationDate = model.UpdateDate.AddDays(_config.DataExpiryDays);
            if (expirationDate <= DateTime.Now)
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
            model.GetType().GetMethod("Load").Invoke(model, new TJson[] { json });
            await _dbManager.Insert(model).ConfigureAwait(false);
            return model;
        }
    }
}