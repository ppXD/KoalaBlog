using KoalaBlog.DAL;
using KoalaBlog.Entity;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL
{
    public abstract class EntityHandlerBase<TEntity> where TEntity : EntityBase
    {
        private readonly DbContext _dbContext;

        public EntityHandlerBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Entities
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual TEntity GetById(params object[] keyValues)
        {
            return _dbContext.Set<TEntity>().Find(keyValues);
        }

        public virtual void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }
        }

        public virtual void Attach(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
        }

        public virtual void MarkAsModified(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public virtual void MarkAsDeleted(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
        }

        public virtual int Count()
        {
            return _dbContext.Set<TEntity>().Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Fetch(predicate).CountAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(predicate);
        }

        public virtual void MarkAsDeleted(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in Fetch(predicate))
            {
                MarkAsDeleted(entity);
            }
        }

        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate, int skip, int take)
        {
            return Fetch(predicate).Skip(skip).Take(take);
        }

        public virtual IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] paths)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            foreach (var path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }
}
