using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oyang.Identity.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.IApplication.Account;
using Oyang.Identity.IApplication.Account.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace Oyang.Identity.WebApi.ApiControllers
{
    [AllowAnonymous]
    public class ErrorController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache  _memoryCache;

        public ErrorController(
            ILogger<AccountController> logger,
            IConfiguration configuration,
            IMemoryCache memoryCache
            )
        {
            _logger = logger;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Problem()
        {
            return Problem();
        }

    }

}
