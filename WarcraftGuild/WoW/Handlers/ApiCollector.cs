using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Helpers;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;
using WarcraftGuild.WoW.Configuration;
using Microsoft.Extensions.Options;

namespace WarcraftGuild.WoW.Handlers
{
    public class ApiCollector : IApiCollector
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly ApiConfiguration _config;

        public ApiCollector(IOptions<ApiConfiguration> apiConfiguration, IBlizzardApiReader blizzardApiReader)
        {
            _config = apiConfiguration.Value;
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
        }

        public async Task<ConnectedRealm> GetConnectedRealmById(ulong blizzardId, bool forceUpdate = false)
        {
            ConnectedRealm connectedRealm = await CheckDbByBlizzardId<ConnectedRealm>(blizzardId, forceUpdate).ConfigureAwait(false);
            if (connectedRealm == null)
                connectedRealm = await DbInsertFromApi<ConnectedRealm, ConnectedRealmJson>($"data/wow/connected-realm/{blizzardId}", Namespace.Dynamic).ConfigureAwait(false);
            return connectedRealm;
        }

        private async Task<TModel> CheckDbByBlizzardId<TModel>(ulong blizzardId, bool forceUpdate = false) where TModel : WoWModel, new()
        {
            Repository repository = new Repository();
            TModel model = forceUpdate ? await repository.GetByBlizzardId<TModel>(blizzardId).ConfigureAwait(false) : null;
            if (model != null && model.UpdateDate.AddDays(_config.DataExpiryDays) > DateTime.Now)
            {
                await repository.Delete(model).ConfigureAwait(false);
                model = null;
            }
            return model;
        }

        private async Task<TModel> DbInsertFromApi<TModel, TJson>(string query, Namespace? ns = null) where TModel : WoWModel, new() where TJson : WoWJson, new()
        {
            Repository repository = new Repository();
            TJson json = await _blizzardApiReader.GetAsync<TJson>(query, ns).ConfigureAwait(false);
            TModel model = new TModel();
            model.Load(json);
            await repository.Insert(model).ConfigureAwait(false);
            return model;
        }

        public async Task<Realm> GetRealmBySlug(string slug, bool forceUpdate = false)
        {
            Repository repository = new Repository();
            Realm realm = forceUpdate ? await repository.GetRealmBySlug(slug).ConfigureAwait(false) : null;
            if (realm != null && realm.UpdateDate.AddDays(_config.DataExpiryDays) > DateTime.Now)
            {
                await repository.Delete(realm).ConfigureAwait(false);
                realm = null;
            }
            if (realm == null)
            {
                realm = await DbInsertFromApi<Realm, RealmJson>($"data/wow/realm/{slug}", Namespace.Dynamic).ConfigureAwait(false);
            }
            return realm;
        }
    }
}