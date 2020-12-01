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

namespace Oyang.Identity.WebApi.ApiControllers
{
    public class AccountController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountAppService _accountAppService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache  _memoryCache;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountAppService accountAppService,
            IConfiguration configuration,
            IMemoryCache memoryCache
            )
        {
            _logger = logger;
            _accountAppService = accountAppService;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public IActionResult GenerateToken(LoginInputDto input)
        {
            var currentUser = _accountAppService.Validate(input);

            var jwtSection = _configuration.GetSection("jwt");
            var issuer = jwtSection.GetValue<string>("issuer");
            var audience = jwtSection.GetValue<string>("audience");
            var securityKey = jwtSection.GetValue<string>("securityKey");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(securityKey));
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, currentUser.LoginName),
                new Claim(JwtRegisteredClaimNames.Sub, currentUser.Id.ToString()),
            };
            var now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                   issuer: issuer,
                   audience: audience,
                   claims: claims,
                   notBefore: now,
                   expires: now.AddMinutes(20),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
               );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            _memoryCache.Set("CurrentUser", currentUser, TimeSpan.FromMinutes(20));
            return Ok(jwtToken);
        }

    }

}
