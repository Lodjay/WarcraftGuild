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
    [Route("wow")]
    [ApiController]
    public class WoWController : ControllerBase
    {
        private readonly IBlizzardApiHandler _blizzardApiHandler;
        private readonly ILogger<WoWController> _logger;

        public WoWController(IBlizzardApiHandler blizzardApiHandler, ILogger<WoWController> logger)
        {
            _blizzardApiHandler = blizzardApiHandler ?? throw new ArgumentNullException(nameof(blizzardApiHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("guild/{guildName}/{realmName}")]
        [HttpGet]
        public async Task<IActionResult> GetGuild([FromRoute] string guildName, [FromRoute] string realmName)
        {
            try
            {
                _logger.LogTrace("Call GetGuild");
                Guild guild = await _blizzardApiHandler.GetGuildBySlug(realmName.Slugify(), guildName.Slugify()).ConfigureAwait(false);
                
                return new JsonResult(guild) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex) when (ex != null)
            {
                _logger.LogCritical(ex.Message);
                return new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}