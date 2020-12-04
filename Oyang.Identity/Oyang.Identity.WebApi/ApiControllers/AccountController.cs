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

namespace Oyang.Identity.WebApi.ApiControllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountAppService _accountAppService;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountAppService accountAppService
            )
        {
            _logger = logger;
            _accountAppService = accountAppService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult<string> GenerateToken(LoginInputDto input)
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
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        [HttpGet]
        public ActionResult<string> GetToken()
        {
            var input = new LoginInputDto() { LoginName = "admin", Password = "123" };
            var token = _accountAppService.GenerateToken(input);
            //if (!_memoryCache.TryGetValue("Token_admin", out string token))
            //{
            //    var input = new LoginInputDto() { LoginName = "admin", Password = "123" };
            //    token = _accountAppService.GenerateToken(input);
            //    _memoryCache.Set("Token_admin", token, TimeSpan.FromMinutes(20));
            //}
            return token;
        }
    }

}
