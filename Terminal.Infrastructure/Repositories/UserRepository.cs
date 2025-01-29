using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Users.Repositories;
using Terminal.Domain;
using Terminal.Domain.Models;

namespace Terminal.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }
        public Task<bool> Exists(string mail, string username)
        {

            User result = _dbSet.FirstOrDefault(e => e.Email.ToLower() == mail.ToLower() || e.UserName.ToLower() == username.ToLower());
            
            return Task.FromResult(result != null);
        }

        public Task<User> GetByMailAsync(string mail, CancellationToken cancellationToken)
        {
            var target = _dbSet.FirstOrDefault(e => e.State != State.Deleted && e.Email.ToLower() == mail.ToLower());
            if (target == null) throw new InexistentEntityException();
            return Task.FromResult(target);
        }
    }
}
