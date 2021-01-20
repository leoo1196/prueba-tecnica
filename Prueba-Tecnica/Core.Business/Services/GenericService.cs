using Core.Contracts.Repositories;
using Core.Contracts.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public abstract class GenericService<TEntity> : IGenericeService<TEntity> where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return await _repository.GetById(keyValues);
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _repository.Insert(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.Update(entity);
        }

        public virtual async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            if (entity != null)
                await _repository.Delete(entity);
        }
    }
}
