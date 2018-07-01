using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SkillTracker.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            dbSet = _entities.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Update(TEntity entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> objects = dbSet.Where<TEntity>(predicate).AsEnumerable();
            foreach (TEntity obj in objects)
            {
                dbSet.Remove(obj);
            }
        }
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> query = dbSet.Where(predicate).AsEnumerable();
            return query;
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable<TEntity>();
        }
        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
