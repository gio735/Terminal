using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Definitions.Repositories;
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
    }
}
