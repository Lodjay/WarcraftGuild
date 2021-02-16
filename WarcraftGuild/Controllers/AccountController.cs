using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.Handlers.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarcraftGuild.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountHandler _accountHandler;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountHandler accountHandler, ILogger<AccountController> logger)
        {
            _accountHandler = accountHandler ?? throw new ArgumentNullException(nameof(accountHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public void CreateAccount([FromQuery] string token)
        {
        }
    }
}
