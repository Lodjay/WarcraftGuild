using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi;
using WarcraftGuild.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarcraftGuild.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IBlizzardApiReader _blizzardApiReader;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IBlizzardApiReader blizzardApiReader, ILogger<AccountController> logger)
        {
            _blizzardApiReader = blizzardApiReader ?? throw new ArgumentNullException(nameof(blizzardApiReader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromQuery] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return new JsonResult("Please add token to Query") { StatusCode = (int)HttpStatusCode.BadRequest };
                await _blizzardApiReader.SendTokenRequest().ConfigureAwait(false);
                return new JsonResult(string.Empty) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (Exception ex) when (ex != null)
            {
                _logger.LogCritical(ex.Message);
                return new JsonResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
