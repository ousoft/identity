﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Oyang.Identity.Domain;
using Oyang.Identity.Domain.Entities;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.Infrastructure.EntityFrameworkCore
{
    public class IdentityDbContext : AuditDbContext
    {
        public IdentityDbContext(DbContextOptions options, ICurrentUser currentUser) 
            : base(options, currentUser)
        {

        }

        //public DbSet<DataDictionaryEntity> DataDictionaries { get; set; }
        //public DbSet<UserEntity> Users { get; set; }
        //public DbSet<RoleEntity> Roles { get; set; }
        //public DbSet<PermissionEntity> Permissions { get; set; }
        //public DbSet<OrgEntity> Orgs { get; set; }
        //public DbSet<MenuEntity> Menus { get; set; }
        //public DbSet<UserRoleEntity> UserRoles { get; set; }
        //public DbSet<UserOrgEntity> UserOrgs { get; set; }
        //public DbSet<RolePermissionEntity> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.OnOyangIdentityModelCreating();
        }

    }
}
