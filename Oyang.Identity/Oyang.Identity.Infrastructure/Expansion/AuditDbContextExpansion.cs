using Oyang.Identity.Domain;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Expansion2
{
    public static class AuditDbContextExpansion
    {
        public static void SetAddAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.IsDeleted = false;
                item.CreationTime = time;
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }
                if (dbContext.CurrentUser.IsAuthenticated)
                {
                    item.CreatorId = dbContext.CurrentUser.Id;
                }
            }
        }
        public static void SetUpdateAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.LastModificationTime = time;
                if (dbContext.CurrentUser.IsAuthenticated)
                {
                    item.LastModifierId = dbContext.CurrentUser.Id;
                }
            }
        }
        public static void SetRemoveAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.IsDeleted = true;
                item.DeletionTime = time;
                if (dbContext.CurrentUser.IsAuthenticated)
                {
                    item.DeleterId = dbContext.CurrentUser.Id;
                }
            }
        }
        public static void SetAddAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity => SetAddAudit(dbContext, new TEntity[] { entity });
        public static void SetUpdateAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity => SetUpdateAudit(dbContext, new TEntity[] { entity });
        public static void SetRemoveAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity => SetRemoveAudit(dbContext, new TEntity[] { entity });

        public static void AddWithAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            SetAddAudit(dbContext, entities);
            dbContext.AddRange(entities);
        }
        public static void AddWithAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity
        {
            SetAddAudit(dbContext, entity);
            dbContext.Add(entity);
        }

        public static void UpdateWithAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            SetUpdateAudit(dbContext, entities);
            dbContext.UpdateRange(entities);
        }
        public static void UpdateWithAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity
        {
            SetUpdateAudit(dbContext, entity);
            dbContext.Update(entity);
        }

        public static void RemoveWithAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<TEntity> entities) where TEntity : Entity => SetRemoveAudit(dbContext, entities);
        public static void RemoveWithAudit<TEntity>(this AuditDbContext dbContext, TEntity entity) where TEntity : Entity => SetRemoveAudit(dbContext, entity);
        public static void RemoveWithAudit<TEntity>(this AuditDbContext dbContext, IEnumerable<Guid> ids) where TEntity : Entity
        {
            var entities = dbContext.Set<TEntity>().Where(t => ids.Contains(t.Id)).ToList();
            SetRemoveAudit(dbContext, entities);
        }
        public static void RemoveWithAudit<TEntity>(this AuditDbContext dbContext, Guid id) where TEntity : Entity
        {
            var entity = dbContext.Set<TEntity>().Find(id);
            SetRemoveAudit(dbContext, entity);
        }
        public static void RemoveWithAudit<TEntity>(this AuditDbContext dbContext, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            var entities = dbContext.Set<TEntity>().Where(predicate).ToList();
            SetRemoveAudit(dbContext,entities);
        }
    }
}
