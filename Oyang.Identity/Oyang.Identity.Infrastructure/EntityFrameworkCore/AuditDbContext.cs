using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Oyang.Identity.Domain;
using System.Linq.Expressions;
using System.Linq;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.Infrastructure.EntityFrameworkCore
{
    public class AuditDbContext : DbContext
    {
        public ICurrentUser CurrentUser { get; }
        public AuditDbContext(DbContextOptions options, ICurrentUser currentUser)
            : base(options)
        {
            CurrentUser = currentUser;
        }
        private void InternalSetAddAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
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
                if (CurrentUser.IsAuthenticated)
                {
                    item.CreatorId = CurrentUser.Id;
                }
            }
        }
        private void InternalSetUpdateAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.LastModificationTime = time;
                if (CurrentUser.IsAuthenticated)
                {
                    item.LastModifierId = CurrentUser.Id;
                }
            }
        }
        private void InternalSetRemoveAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.IsDeleted = true;
                item.DeletionTime = time;
                if (CurrentUser.IsAuthenticated)
                {
                    item.DeleterId = CurrentUser.Id;
                }
            }
        }

        public void SetAddAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity => InternalSetAddAudit(entities);
        public void SetAddAudit<TEntity>(TEntity entity) where TEntity : Entity => InternalSetAddAudit(new TEntity[] { entity });
        public void SetUpdateAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity => InternalSetUpdateAudit(entities);
        public void SetUpdateAudit<TEntity>(TEntity entity) where TEntity : Entity => InternalSetUpdateAudit(new TEntity[] { entity });
        public void SetRemoveAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity => InternalSetRemoveAudit(entities);
        public void SetRemoveAudit<TEntity>(TEntity entity) where TEntity : Entity => InternalSetRemoveAudit(new TEntity[] { entity });

        public void AddWithAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            InternalSetAddAudit(entities);
            base.AddRange(entities);
        }
        public void AddWithAudit<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entities = new TEntity[] { entity };
            InternalSetAddAudit(entities);
            base.AddRange(entities);
        }
        public void UpdateWithAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            InternalSetUpdateAudit(entities);
            base.UpdateRange(entities);
        }
        public void UpdateWithAudit<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entities = new TEntity[] { entity };
            InternalSetUpdateAudit(entities);
            base.UpdateRange(entities);
        }
        public void RemoveWithAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity => InternalSetRemoveAudit(entities);
        public void RemoveWithAudit<TEntity>(TEntity entity) where TEntity : Entity => InternalSetRemoveAudit(new TEntity[] { entity });
        public void RemoveWithAudit<TEntity>(IEnumerable<Guid> ids) where TEntity : Entity
        {
            var entities = base.Set<TEntity>().Where(t => ids.Contains(t.Id)).ToList();
            InternalSetRemoveAudit(entities);
        }
        public void RemoveWithAudit<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = base.Set<TEntity>().Find(id);
            var entities = new TEntity[] { entity };
            InternalSetRemoveAudit(entities);
        }
        public void RemoveWithAudit<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            var entities = base.Set<TEntity>().Where(predicate).ToList();
            InternalSetRemoveAudit(entities);
        }
    }
}
