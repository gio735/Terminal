using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Application.Definitions.Repositories
{
    public interface IDefinitionRepository
    {
        public Task<Definition?> Exists(string georgianTitle, string englishTitle);
        public Task AddAsync(Definition entity, CancellationToken cancellationToken);
        public Task UpdateAsync(Definition entity, CancellationToken cancellationToken);
        public Task<Definition> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        public Task<IQueryable<Definition>> GetAll(CancellationToken cancellationToken);
    }
}
