using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.Core.Helpers;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.Controllers
{
    [Route("init")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IApiInitializer _apiInitializer;
        private readonly ILogger<WoWController> _logger;

        public InitController(IApiInitializer apiInitializer, ILogger<WoWController> logger)
        {
            _apiInitializer = apiInitializer ?? throw new ArgumentNullException(nameof(apiInitializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> InitApi()
        {
            try
            {
                _logger.LogTrace("Initialize API...");
                await _apiInitializer.Init().ConfigureAwait(false);
                return new OkResult();
            }
            catch (Exception ex) when (ex != null)
            {
                _logger.LogCritical(ex.Message);
                return new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [Route("guild/{guildName}/{realmName}")]
        [HttpGet]
        public async Task<IActionResult> InitGuild([FromRoute] string guildName, [FromRoute] string realmName)
        {
            try
            {
                _logger.LogTrace($"Initialize API guild {guildName} from {realmName}...");
                await _apiInitializer.InitGuild(realmName.Slugify(), guildName.Slugify()).ConfigureAwait(false);
                return new OkResult();
            }
            catch (Exception ex) when (ex != null)
            {
                _logger.LogCritical(ex.Message);
                return new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}