using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Oyang.Identity.Domain;
using Oyang.Identity.Domain.Entities;
using Oyang.Identity.IApplication.Account;
using Oyang.Identity.IApplication.Account.Dtos;
using Oyang.Identity.Infrastructure.Common;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using Oyang.Identity.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Oyang.Identity.Application.Account
{
    public class AccountAppService : BaseAppService, IAccountAppService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAppSettings _appSettings;
        private readonly IMemoryCache _memoryCache;
        public AccountAppService(
            IdentityDbContext dbContext,
            IAppSettings appSettings,
            IMemoryCache memoryCache
            )
        {
            _dbContext = dbContext;
            _appSettings = appSettings;
            _memoryCache = memoryCache;
        }

        public string GenerateToken(LoginInputDto input)
        {
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.LoginName), "登录名不能为空");
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password), "密码不能为空");
            var userEntity = _dbContext.Set<UserEntity>().SingleOrDefault(t => t.LoginName == input.LoginName);
            ValidationObject.Validate(userEntity == null, "登录名不存在");
            var passwordHash = HashAlgorithmHelper.ComputeMD5(input.Password);
            ValidationObject.Validate(userEntity.PasswordHash != passwordHash, "密码不正确");
            if (!_memoryCache.TryGetValue($"CurrentUser_{userEntity.Id}", out CurrentUser currentUser))
            {
                currentUser = Get(input.LoginName);
                _memoryCache.Set($"CurrentUser_{userEntity.Id}", currentUser, TimeSpan.FromMinutes(20));
            }
            var jwtToken = CreateToken(userEntity.LoginName);
            return jwtToken;
        }

        public string RefreshToken(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.SecurityKey)),
                ValidateLifetime = false
            }, out SecurityToken securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;
            if (DateTime.UtcNow > jwtSecurityToken.ValidTo.AddMinutes(10))
            {
                throw new Exception("token刷新超时，请重新登录");
            }
            var name = jwtSecurityToken.Claims.SingleOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub);
            var jwtToken = CreateToken(name.Value);
            return jwtToken;
        }

        private CurrentUser Get(string loginName)
        {
            var entity = _dbContext.Set<UserEntity>().Single(t => t.LoginName == loginName);
            var roleIds = _dbContext.Set<UserRoleEntity>().AsNoTracking().Where(t => t.UserId == entity.Id).Select(t => t.RoleId).ToList();
            var roles = _dbContext.Set<RoleEntity>().AsNoTracking().Where(t => roleIds.Contains(t.Id)).Select(t => t.Name).ToList();
            var permissionIds = _dbContext.Set<RolePermissionEntity>().AsNoTracking().Where(t => roleIds.Contains(t.RoleId)).Select(t => t.PermissionId).Distinct().ToList();
            var permissions = _dbContext.Set<PermissionEntity>().AsNoTracking().Where(t => permissionIds.Contains(t.Id)).Select(t => t.Code).ToList();

            var currentUser = new CurrentUser(entity.Id, entity.LoginName, roles, permissions);
            return currentUser;
        }

        private string CreateToken(string name)
        {
            var now = DateTime.Now;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Jwt.SecurityKey));
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(ClaimTypes.Name, name),
            };
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _appSettings.Jwt.Issuer,
                audience: _appSettings.Jwt.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(10),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
               );
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return jwtToken;
        }
    }
}
