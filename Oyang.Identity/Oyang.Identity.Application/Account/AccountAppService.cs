using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oyang.Identity.IApplication;
using Oyang.Identity.Domain;
using Oyang.Identity.IApplication.Account;
using Oyang.Identity.IApplication.Account.Dtos;
using Oyang.Identity.Infrastructure.Utility;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using Oyang.Identity.Domain.Entities;
using Oyang.Identity.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Oyang.Identity.Application.Account
{
    public class AccountAppService : BaseAppService, IAccountAppService
    {
        private readonly IdentityDbContext _dbContext;
        public AccountAppService(
            IdentityDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public CurrentUser Get(string loginName)
        {
            var entity = _dbContext.Set<UserEntity>().Single(t => t.LoginName == loginName);
            var roleIds = _dbContext.Set<UserRoleEntity>().AsNoTracking().Where(t => t.UserId == entity.Id).Select(t => t.RoleId).ToList();
            var roles = _dbContext.Set<RoleEntity>().AsNoTracking().Where(t => roleIds.Contains(t.Id)).Select(t => t.Name).ToList();
            var permissionIds = _dbContext.Set<RolePermissionEntity>().AsNoTracking().Where(t => roleIds.Contains(t.RoleId)).Select(t => t.PermissionId).Distinct().ToList();
            var permissions = _dbContext.Set<PermissionEntity>().AsNoTracking().Where(t => permissionIds.Contains(t.Id)).Select(t => t.Code).ToList();

            var currentUser = new CurrentUser(entity.Id, entity.LoginName, roles, permissions);
            return currentUser;
        }

        public CurrentUser Validate(LoginInputDto input)
        {
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.LoginName), "登录名不能为空");
            ValidationObject.Validate(string.IsNullOrWhiteSpace(input.Password), "密码不能为空");
            var userEntity = _dbContext.Set<UserEntity>().SingleOrDefault(t => t.LoginName == input.LoginName);
            ValidationObject.Validate(userEntity == null, "登录名不存在");
            var passwordHash = HashAlgorithmHelper.ComputeMD5(input.Password);
            ValidationObject.Validate(userEntity.PasswordHash != passwordHash, "密码不正确");
            var currentUser = Get(input.LoginName);
            return currentUser;
        }

    }
}
