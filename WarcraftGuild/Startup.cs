using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.BlizzardApi.Configuration;
using WarcraftGuild.BlizzardApi.Interfaces;
using WarcraftGuild.WoW.Configuration;
using WarcraftGuild.WoW.Handlers;
using WarcraftGuild.WoW.Interfaces;
using Serilog;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using WarcraftGuild.WoWHeadApi.Configuration;
using WarcraftGuild.WoWHeadApi;

namespace WarcraftGuild
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IWebHostEnvironment hostEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostEnv.EnvironmentName}.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
            string fileName = "logs.log";
            string rootPath = Path.GetPathRoot(Assembly.GetEntryAssembly().Location);
            string DbName = Configuration.GetSection("ApiConfiguration").GetSection("DataBaseName").Value;
            string filePath = Path.Combine(rootPath, "Logs", DbName, fileName);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(filePath)
                //.WriteTo.MongoDB($"mongodb://localhost:27017/{DbName}", "Logs")
                .CreateLogger();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddControllers();
            services.Configure<BlizzardApiConfiguration>(config => Configuration.GetSection("BlizzardApi").Bind(config));
            services.Configure<WoWHeadApiConfiguration>(config => Configuration.GetSection("WoWHeadApi").Bind(config));
            services.Configure<ApiConfiguration>(config => Configuration.GetSection("ApiConfiguration").Bind(config));
            services.AddHttpClient();
            services.AddSingleton<IBlizzardApiReader, BlizzardApiReader>();
            services.AddSingleton<IWoWHeadApiReader, WoWHeadApiReader>();
            services.AddSingleton<IApiInitializer, ApiInitializer>();
            services.AddSingleton<IBlizzardApiHandler, BlizzardApiHandler>();
            services.AddSingleton<IWebClient, ApiWebClient>();
            services.AddSingleton<IDbManager, DbManager>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WarcraftGuild", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WarcraftGuild v1"));
            }

            logger.AddSerilog();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}