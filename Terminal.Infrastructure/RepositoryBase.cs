using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain;

namespace Terminal.Infrastructure
{
    public abstract class RepositoryBase<T> where T : EntityModel
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual bool Exists(T entity)
        {
            return _dbSet.Any(e => e.Id == entity.Id);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            var actualEntity = await GetByIdAsync(cancellationToken, entity.Id);
            if (actualEntity.State == State.Deleted) throw new AttemptToUseDeletedEntityException();
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }
        public virtual async Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] key)
        {
            T result = await _dbSet.FindAsync(key, cancellationToken);
            if (result == null) throw new InexistentEntityException();
            if (result.State == State.Deleted) throw new AttemptToUseDeletedEntityException();
            return result;
        }
        public virtual Task<IQueryable<T>> GetAll(CancellationToken cancellationToken)
        {
            return Task.FromResult(_dbSet.AsNoTracking());
        }
        public virtual async Task DeleteAsync(CancellationToken cancellationToken, params object[] key)
        {
            var entity = await GetByIdAsync(cancellationToken, key);
            if (entity.State == State.Deleted) throw new AttemptToUseDeletedEntityException();
            entity.DeletionDate = DateTime.UtcNow.AddHours(4);
            entity.State = State.Deleted;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task DeleteRangeAsync(List<T> entities, CancellationToken cancellationToken)
        {
            foreach (var entity in entities)
            {
                if (entity.State == State.Deleted) continue;
                entity.DeletionDate = DateTime.UtcNow.AddHours(4);
                entity.State = State.Deleted;
            }
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
