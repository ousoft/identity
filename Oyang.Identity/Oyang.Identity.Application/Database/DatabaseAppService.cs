using Oyang.Identity.Domain.Entities;
using Oyang.Identity.IApplication.Database;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oyang.Identity.Infrastructure.Expansion;

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
        }

        public void GenerateSeedDataByUser()
        {
            var list = new List<UserEntity>();
            list.Add(new UserEntity() { Id = new Guid("430160D6-F0D9-4560-9D12-6B4764E34C6B"), LoginName = "admin", PasswordHash = "123" });
            for (int i = 0; i < 133; i++)
            {
                list.Add(new UserEntity() { LoginName = "testuser" + i.ToString("000"), PasswordHash = "123" });
            }
            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        public void GenerateSeedDataByRole()
        {
            var list = new List<RoleEntity>();
            list.Add(new RoleEntity() { Id = new Guid("51B68BC9-E28E-4315-8D9E-43CA02553472"), Name = "管理员" });
            list.Add(new RoleEntity() { Id = new Guid("FF82F4FB-34E0-4906-A3FF-ABD393D976F8"), Name = "普通用户" });
            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
        public void GenerateSeedDataByUserRole()
        {
            var list = new List<UserRoleEntity>();

            var user = _dbContext.Set<UserEntity>().Single(t => t.LoginName == "admin");
            var role = _dbContext.Set<RoleEntity>().Single(t => t.Name == "管理员");
            list.Add(new UserRoleEntity() { UserId = user.Id, RoleId = role.Id });



            _dbContext.AddWithAudit(list);
            _dbContext.SaveChanges();
        }
    }
}
