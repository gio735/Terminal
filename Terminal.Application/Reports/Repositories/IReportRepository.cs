using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Application.Reports.Repositories
{
    public interface IReportRepository
    {
        public Task AddAsync(Report entity, CancellationToken cancellationToken);
        public Task UpdateAsync(Report entity, CancellationToken cancellationToken);
        public Task<Report> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        public Task<IQueryable<Report>> GetAll(CancellationToken cancellationToken);
    }
}
