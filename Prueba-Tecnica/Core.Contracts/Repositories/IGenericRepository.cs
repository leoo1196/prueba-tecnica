using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Contracts.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(params object[] keyValues);

        Task<ICollection<TEntity>> GetAll();

        Task Insert(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task<ICollection<TEntity>> GetEntities(Expression<Func<TEntity, bool>> predicate, string includedProperties = null);
    }
}
