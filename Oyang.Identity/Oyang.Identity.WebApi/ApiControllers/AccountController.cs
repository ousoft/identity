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
using Microsoft.AspNetCore.Http;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.WebApi.ApiControllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountAppService _accountAppService;
        private readonly IMemoryCache _memoryCache;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountAppService accountAppService,
            IMemoryCache memoryCache
            )
        {
            _logger = logger;
            _accountAppService = accountAppService;
            _memoryCache = memoryCache;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public string GenerateToken(LoginInputDto input)
        {
            return _accountAppService.GenerateToken(input);
        }

        [HttpPost]
        public ActionResult<string> RefreshToken(string token)
        {
            try
            {
                return _accountAppService.RefreshToken(token);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        [Authorize]
        [HttpGet]
        public CurrentUser UserInfo()
        {
            var memoryKey = $"CurrentUser_{User.Identity.Name}";
            if (!_memoryCache.TryGetValue(memoryKey, out CurrentUser currentUser))
            {
                currentUser = _accountAppService.GetCurrentUser(User.Identity.Name);
                _memoryCache.Set(memoryKey, currentUser, TimeSpan.FromMinutes(20));
            }
            return currentUser;
        }
    }

}
