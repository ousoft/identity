using Oyang.Identity.Domain.Entities;
using Oyang.Identity.IApplication.Database;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oyang.Identity.Infrastructure.Expansion;
using Oyang.Identity.Infrastructure.Common;
using Oyang.Identity.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;

namespace Oyang.Identity.Application.Database
{
    public class DatabaseAppService : BaseAppService, IDatabaseAppService
    {
        private readonly IdentityDbContext _dbContext;
        public DatabaseAppService(
            IdentityDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }


        public bool GenerateDatabase()
        {
            return _dbContext.Database.EnsureCreated();
        }

        public void GenerateSeedData()
        {
            GenerateSeedDataByUser();
            GenerateSeedDataByRole();
            GenerateSeedDataByUserRole();
            GenerateSeedDataByPermission();
            GenerateSeedDataByRolePermission();
        }

        private void GenerateSeedDataByUser()
        {
            var list = new List<UserEntity>();
            var passwordHash = HashAlgorithmHelper.ComputeMD5("123");
            list.Add(new UserEntity() { Id = new Guid("430160D6-F0D9-4560-9D12-6B4764E34C6B"), LoginName = "admin", PasswordHash = passwordHash });
            for (int i = 0; i < 133; i++)
            {
                list.Add(new UserEntity() { LoginName = "testuser" + i.ToString("000"), PasswordHash = passwordHash });
            }
            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        private void GenerateSeedDataByRole()
        {
            var list = new List<RoleEntity>();
            list.Add(new RoleEntity() { Id = new Guid("51B68BC9-E28E-4315-8D9E-43CA02553472"), Name = "管理员" });
            list.Add(new RoleEntity() { Id = new Guid("FF82F4FB-34E0-4906-A3FF-ABD393D976F8"), Name = "普通用户" });
            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        private void GenerateSeedDataByUserRole()
        {
            var list = new List<UserRoleEntity>();

            var user = _dbContext.Set<UserEntity>().Single(t => t.LoginName == "admin");
            var role = _dbContext.Set<RoleEntity>().Single(t => t.Name == "管理员");
            list.Add(new UserRoleEntity() { UserId = user.Id, RoleId = role.Id });



            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        private void GenerateSeedDataByPermission()
        {
            var list = new List<PermissionEntity>();

            var fields = typeof(PermissionNames).GetFields();
            foreach (var field in fields)
            {
                list.Add(new PermissionEntity()
                {
                    Code = field.Name,
                    Name = field.GetRawConstantValue().ToString()
                });
            }

            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        private void GenerateSeedDataByRolePermission()
        {
            var list = new List<RolePermissionEntity>();

            var role = _dbContext.Set<RoleEntity>().Single(t => t.Name == "管理员");
            var adminPermissions = _dbContext.Set<PermissionEntity>().AsNoTracking().Select(t => t.Id)
                .Select(t => new RolePermissionEntity()
                {
                    RoleId = role.Id,
                    PermissionId = t
                }).ToList();
            list.AddRange(adminPermissions);

            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }



    }
}
