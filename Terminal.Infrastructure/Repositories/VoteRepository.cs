using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Votes.Repositories;
using Terminal.Domain;
using Terminal.Domain.Models;
using Terminal.Domain.Utilities;

namespace Terminal.Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<Vote> _dbSet;

        public VoteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Vote>();
        }

        public async Task AddAsync(Vote entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken, int userId, int definitionId)
        {
            var entity = await _dbSet.FindAsync(new {userId,  definitionId});
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<IQueryable<Vote>> GetAll(CancellationToken cancellationToken)
        {
            return Task.FromResult(_dbSet.AsNoTracking());
        }

        public async Task<Vote> GetByIdAsync(CancellationToken cancellationToken, int userId, int definitionId)
        {
            var result = await _dbSet.FindAsync(new { userId, definitionId });
            return result;
        }

        public async Task UpdateAsync(Vote entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
