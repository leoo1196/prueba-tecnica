using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GenericRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private DbContext GetContext() => _contextFactory.CreateDbContext();

        public async Task<TEntity> GetById(params object[] keyValues)
        {
            using var db = GetContext();
            return await db.Set<TEntity>().FindAsync(keyValues);
        }

        public async Task<ICollection<TEntity>> GetAll()
        {
            using var db = GetContext();
            return await db.Set<TEntity>().ToListAsync();
        }

        public async Task Insert(TEntity entity)
        {
            using var db = GetContext();
            db.Set<TEntity>().Add(entity);
            await db.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            using var db = GetContext();
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            using var db = GetContext();
            db.Set<TEntity>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task<ICollection<TEntity>> GetEntities(Expression<Func<TEntity, bool>> predicate, string includedProperties = null)
        {
            using var db = GetContext();

            var set = db.Set<TEntity>().AsQueryable();

            set = set.Where(predicate);

            if (includedProperties != null)
            {
                var properties = includedProperties.Split(",").Select(e => e.Trim()).ToList();

                foreach (var property in properties)
                {
                    set = set.Include(property);
                }
            }

            return await set.ToListAsync();
        }
    }
}
