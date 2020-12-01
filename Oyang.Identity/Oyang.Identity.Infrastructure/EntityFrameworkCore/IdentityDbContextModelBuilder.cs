using Microsoft.EntityFrameworkCore;
using Oyang.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.Infrastructure.EntityFrameworkCore
{
    public static class IdentityDbContextModelBuilder
    {
        public static void OnOyangIdentityModelCreating(this ModelBuilder modelBuilder)
        {
            AddEntityType(modelBuilder);
            SetTableName(modelBuilder);
            SetQueryFilter(modelBuilder);
        }

        private static void AddEntityType(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.AddEntityType(typeof(DataDictionaryEntity));
            modelBuilder.Model.AddEntityType(typeof(UserEntity));
            modelBuilder.Model.AddEntityType(typeof(RoleEntity));
            modelBuilder.Model.AddEntityType(typeof(UserRoleEntity));
            modelBuilder.Model.AddEntityType(typeof(PermissionEntity));
            modelBuilder.Model.AddEntityType(typeof(RolePermissionEntity));
            modelBuilder.Model.AddEntityType(typeof(OrgEntity));
            modelBuilder.Model.AddEntityType(typeof(MenuEntity));
            modelBuilder.Model.AddEntityType(typeof(UserOrgEntity));
        }

        private static void SetTableName(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var entityTypeBuilder = modelBuilder.Entity(entity.Name);
                string tableName = "T_Identity_" + entity.ClrType.Name.Replace("Entity", "");
                entityTypeBuilder.ToTable(tableName);

                //var properties = entity.GetProperties();
                //foreach (var property in properties)
                //{
                //    var propertyBuilder = entityTypeBuilder.Property(property.Name);
                //    propertyBuilder.HasColumnName(property.Name.ToLower());
                //    if (property.Name == "Id")
                //    {
                //        propertyBuilder.HasColumnType("char(36)");
                //    }
                //    else if (property.ClrType == typeof(string))
                //    {
                //        propertyBuilder.HasColumnType("varchar(255)");
                //    }
                //}

            }
        }
        private static void SetQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataDictionaryEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<UserEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<RoleEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<PermissionEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<OrgEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<MenuEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<UserRoleEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<UserOrgEntity>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<RolePermissionEntity>().HasQueryFilter(t => !t.IsDeleted);
        }

    }
}
