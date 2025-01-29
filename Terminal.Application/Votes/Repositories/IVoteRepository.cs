using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;
using Terminal.Domain.Utilities;

namespace Terminal.Application.Votes.Repositories
{
    public interface IVoteRepository
    {
        public Task AddAsync(Vote entity, CancellationToken cancellationToken);
        public Task UpdateAsync(Vote entity, CancellationToken cancellationToken);
        public Task<Vote> GetByIdAsync(CancellationToken cancellationToken, int userId, int definitionId);
        public Task DeleteAsync(CancellationToken cancellationToken, int userId, int definitionId);
        public Task<IQueryable<Vote>> GetAll(CancellationToken cancellationToken);
    }
}
