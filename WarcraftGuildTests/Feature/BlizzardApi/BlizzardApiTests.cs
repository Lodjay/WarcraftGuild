using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using Xunit;
using Autofac;
using System.IO;
using Microsoft.Extensions.Options;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Enums;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using System.Net;

namespace WarcraftGuildTests.Feature.BlizzardApi
{
    public class BlizzardApiTests
    {
        private IConfigurationRoot Configuration { get; set; }
        private IBlizzardApiReader Api { get; set; }

        public BlizzardApiTests()
        {
            InitConfig();
            InitApi();
        }

        private void InitConfig()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private void InitApi()
        {
            ContainerBuilder builder = new AutofacServiceProviderFactory().CreateBuilder(new ServiceCollection().AddHttpClient());

            builder.RegisterType<BlizzardApiReader>().As<IBlizzardApiReader>();
            builder.RegisterType<ApiWebClient>().As<IWebClient>();
            builder.Register(c => Options.Create(Configuration.GetSection("BlizzardApi").Get<BlizzardApiConfiguration>())).As<IOptions<BlizzardApiConfiguration>>();

            Api = builder.Build().Resolve<IBlizzardApiReader>();
        }

        [Fact]
        public async Task GetRealmIndex_Test()
        {
            string query = "data/wow/realm/index";
            RealmIndexJson result = await Api.GetAsync<RealmIndexJson>(query, Namespace.Dynamic).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.Realms.Count > 0);
        }

        [Fact]
        public async Task GetRealm_Test()
        {
            string realmSlug = "hyjal";
            string query = $"data/wow/realm/{realmSlug}";
            RealmJson result = await Api.GetAsync<RealmJson>(query, Namespace.Dynamic).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(realmSlug, result.Slug);
        }

        [Fact]
        public async Task GetConnectedRealmIndex_Test()
        {
            string query = "data/wow/connected-realm/index";
            ConnectedRealmIndexJson result = await Api.GetAsync<ConnectedRealmIndexJson>(query, Namespace.Dynamic).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.ConnectedRealms.Count > 0);
        }

        [Fact]
        public async Task GetConnectedRealm_Test()
        {
            uint id = 1390;
            string query = $"data/wow/connected-realm/{id}";
            ConnectedRealmJson result = await Api.GetAsync<ConnectedRealmJson>(query, Namespace.Dynamic).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetAchievementIndex_Test()
        {
            string query = "data/wow/achievement/index";
            AchievementIndexJson result = await Api.GetAsync<AchievementIndexJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.Achievements.Count > 0);
        }

        [Fact]
        public async Task GetAchievement_Test()
        {
            uint id = 7380;
            string query = $"data/wow/achievement/{id}";
            AchievementJson result = await Api.GetAsync<AchievementJson>(query, Namespace.Static).ConfigureAwait(false); 
            MediaJson media = await Api.GetAsync<MediaJson>($"data/wow/media/achievement/{id}", Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
            Assert.Equal(HttpStatusCode.OK, media.ResultCode);
            Assert.True(media.DirectlyCalled);
            Assert.NotEmpty(media.Assets);
            Assert.NotNull(media.Assets.Find(x => x.Key == "icon").Key);
        }

        [Fact]
        public async Task GetAchievementCategoryIndex_Test()
        {
            string query = "data/wow/achievement-category/index";
            AchievementCategoryIndexJson result = await Api.GetAsync<AchievementCategoryIndexJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.Categories.Count > 0);
        }

        [Fact]
        public async Task GetAchievementCategory_Test()
        {
            uint id = 81;
            string query = $"data/wow/achievement-category/{id}";
            AchievementCategoryJson result = await Api.GetAsync<AchievementCategoryJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetRaceIndex_Test()
        {
            string query = "data/wow/playable-race/index";
            RaceIndexJson result = await Api.GetAsync<RaceIndexJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.Races.Count > 0);
        }

        [Fact]
        public async Task GetRace_Test()
        {
            uint id = 1;
            string query = $"data/wow/playable-race/{id}";
            RaceJson result = await Api.GetAsync<RaceJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetClassIndex_Test()
        {
            string query = "data/wow/playable-class/index";
            ClassIndexJson result = await Api.GetAsync<ClassIndexJson>(query, Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.True(result.Classes.Count > 0);
        }

        [Fact]
        public async Task GetClass_Test()
        {
            uint id = 1;
            string query = $"data/wow/playable-class/{id}";
            ClassJson result = await Api.GetAsync<ClassJson>(query, Namespace.Static).ConfigureAwait(false);
            MediaJson media = await Api.GetAsync<MediaJson>($"data/wow/media/playable-class/{id}", Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
            Assert.Equal(HttpStatusCode.OK, media.ResultCode);
            Assert.True(media.DirectlyCalled);
            Assert.NotEmpty(media.Assets);
            Assert.NotNull(media.Assets.Find(x => x.Key == "icon").Key);
        }

        [Fact]
        public async Task GetSpec_Test()
        {
            uint id = 71;
            string query = $"data/wow/playable-specialization/{id}";
            SpecializationJson result = await Api.GetAsync<SpecializationJson>(query, Namespace.Static).ConfigureAwait(false);
            MediaJson media = await Api.GetAsync<MediaJson>($"data/wow/media/playable-specialization/{id}", Namespace.Static).ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.ResultCode);
            Assert.True(result.DirectlyCalled);
            Assert.Equal(id, result.Id);
            Assert.Equal(HttpStatusCode.OK, media.ResultCode);
            Assert.True(media.DirectlyCalled);
            Assert.NotEmpty(media.Assets);
            Assert.NotNull(media.Assets.Find(x => x.Key == "icon").Key);
        }
    }
}
