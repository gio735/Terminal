using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Definitions.Repositories;
using Terminal.Domain;
using Terminal.Domain.Models;

namespace Terminal.Infrastructure.Repositories
{
    public class DefinitionRepository : RepositoryBase<Definition>, IDefinitionRepository
    {
        public DefinitionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<Definition?> Exists(string georgianTitle, string englishTitle)
        {
            var target = _dbSet.FirstOrDefault(e => (e.GeorgianTitle == georgianTitle || e.EnglishTitle == englishTitle) && e.State != Domain.State.Deleted);
            return Task.FromResult(target);
        }
        public override Task<IQueryable<Definition>> GetAll(CancellationToken cancellationToken)
        {
            return Task.FromResult(_dbSet.Include(e => e.Similars).AsNoTracking());
        }
        public override async Task<Definition> GetByIdAsync(CancellationToken cancellationToken, params object[] key)
        {
            Definition result = await _dbSet.FindAsync(key, cancellationToken);
            if (result == null) throw new InexistentEntityException();
            if (result.State == State.Deleted) throw new AttemptToUseDeletedEntityException();
            return await _dbSet.Include(e => e.Similars).FirstAsync(e => e.Id == result.Id);
        }
    }
}
