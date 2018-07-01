using System;
using System.Collections.Generic;
using SkillTracker.Repositories;

namespace SkillTracker.Services
{
    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
    {
        IUnitOfWork _unitOfWork;
        IGenericRepository<TEntity> _repository;

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


        public virtual TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _unitOfWork.Commit();
            return entity;
        }


        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Update(entity);
            _unitOfWork.Commit();
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
