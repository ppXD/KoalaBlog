using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class EntityConcurrentLockHandlerBase
    {
        private readonly DbContext _dbContext;

        public EntityConcurrentLockHandlerBase(DbContext dbContext)          
        {
            _dbContext = dbContext;
        }

        public virtual async Task<EntityConcurrentLock> GetByIdAsync(object id)
        {
            return await _dbContext.Set<EntityConcurrentLock>().FindAsync(id);
        }

        public virtual void Add(EntityConcurrentLock entity)
        {
            _dbContext.Set<EntityConcurrentLock>().Add(entity);
        }

        public virtual void MarkAsModified(EntityConcurrentLock entity)
        {
            _dbContext.Entry<EntityConcurrentLock>(entity).State = EntityState.Modified;
        }

        public virtual void MarkAsDeleted(EntityConcurrentLock entity)
        {
            _dbContext.Entry<EntityConcurrentLock>(entity).State = EntityState.Deleted;
        }
    }
}
