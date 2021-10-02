using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.WoW.Handlers;
using WarcraftGuild.WoW.Interfaces;
using WarcraftGuild.WoW.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarcraftGuild.Controllers
{
    [Route("wow")]
    [ApiController]
    public class WoWController : ControllerBase
    {
        private readonly IWoWHandler _WoWHandler;
        private readonly ILogger<WoWController> _logger;

        public WoWController(IWoWHandler WoWHandler, ILogger<WoWController> logger)
        {
            _WoWHandler = WoWHandler ?? throw new ArgumentNullException(nameof(WoWHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("guild/{nameSlug}/{realmSlug}")]
        [HttpGet]
        public async Task<IActionResult> GetGuild([FromRoute] string nameSlug, [FromRoute] string realmSlug)
        {
            try
            {
                Guild guild = await _WoWHandler.GetGuild(realmSlug, nameSlug, true).ConfigureAwait(false);
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
