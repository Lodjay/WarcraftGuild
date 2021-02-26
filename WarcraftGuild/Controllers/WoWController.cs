using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.Enums;
using WarcraftGuild.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarcraftGuild.Controllers
{
    [Route("wow")]
    [ApiController]
    public class WoWController : ControllerBase
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly ILogger<AccountController> _logger;

        public WoWController(IBlizzardApiReader blizzardApiReader, ILogger<AccountController> logger)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetQuery([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return new JsonResult("Please add Query") { StatusCode = (int)HttpStatusCode.BadRequest };
                var test = await _blizzardApiReader.GetJsonAsync(query, Namespace.Static).ConfigureAwait(false);
                return new JsonResult(test) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex) when (ex != null)
            {
                _logger.LogCritical(ex.Message);
                return new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
