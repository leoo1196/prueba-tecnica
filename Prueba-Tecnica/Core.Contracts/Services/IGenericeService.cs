using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Contracts.Services
{
    public interface IGenericeService<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(params object[] keyValues);

        Task<ICollection<TEntity>> GetAllAsync();

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(params object[] keyValues);
    }
}
