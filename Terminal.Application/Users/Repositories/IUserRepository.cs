using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Application.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> Exists(string mail, string username);
        public Task AddAsync(User entity, CancellationToken cancellationToken);
        public Task UpdateAsync(User entity, CancellationToken cancellationToken);
        public Task<User> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task<User> GetByMailAsync(string mail, CancellationToken cancellationToken);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        public Task<IQueryable<User>> GetAll(CancellationToken cancellationToken);
    }
}
