using Oyang.Identity.Domain;
using Oyang.Identity.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.EntityFrameworkCore
{
    public class AuditManage
    {
        private readonly AuditDbContext _dbContext;
        public AuditManage(AuditDbContext dbContext)
        {
            _dbContext = dbContext;
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
                if (_dbContext.CurrentUser.IsAuthenticated)
                {
                    item.CreatorId = _dbContext.CurrentUser.Id;
                }
            }
        }
        private void InternalSetUpdateAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            var time = DateTime.Now;
            foreach (var item in entities)
            {
                item.LastModificationTime = time;
                if (_dbContext.CurrentUser.IsAuthenticated)
                {
                    item.LastModifierId = _dbContext.CurrentUser.Id;
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
                if (_dbContext.CurrentUser.IsAuthenticated)
                {
                    item.DeleterId = _dbContext.CurrentUser.Id;
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
            _dbContext.AddRange(entities);
        }
        public void AddWithAudit<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entities = new TEntity[] { entity };
            InternalSetAddAudit(entities);
            _dbContext.AddRange(entities);
        }
        public void UpdateWithAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            InternalSetUpdateAudit(entities);
            _dbContext.UpdateRange(entities);
        }
        public void UpdateWithAudit<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entities = new TEntity[] { entity };
            InternalSetUpdateAudit(entities);
            _dbContext.UpdateRange(entities);
        }
        public void RemoveWithAudit<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity => InternalSetRemoveAudit(entities);
        public void RemoveWithAudit<TEntity>(TEntity entity) where TEntity : Entity => InternalSetRemoveAudit(new TEntity[] { entity });
        public void RemoveWithAudit<TEntity>(IEnumerable<Guid> ids) where TEntity : Entity
        {
            var entities = _dbContext.Set<TEntity>().Where(t => ids.Contains(t.Id)).ToList();
            InternalSetRemoveAudit(entities);
        }
        public void RemoveWithAudit<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            var entities = new TEntity[] { entity };
            InternalSetRemoveAudit(entities);
        }
        public void RemoveWithAudit<TEntity>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            var entities = _dbContext.Set<TEntity>().Where(predicate).ToList();
            InternalSetRemoveAudit(entities);
        }
    }
}
