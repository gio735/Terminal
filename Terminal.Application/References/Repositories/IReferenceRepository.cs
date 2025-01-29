using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Application.References.Repositories
{
    public interface IReferenceRepository
    {
        public Task AddAsync(Reference entity, CancellationToken cancellationToken);
        public Task UpdateAsync(Reference entity, CancellationToken cancellationToken);
        public Task<Reference> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        public Task<IQueryable<Reference>> GetAll(CancellationToken cancellationToken);
    }
}
