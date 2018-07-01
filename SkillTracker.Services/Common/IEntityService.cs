using System.Collections.Generic;

namespace SkillTracker.Services
{
    public interface IEntityService<TEntity> : IService where TEntity : class
    {
        TEntity Create(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity Update(TEntity entity);
    }
}
