using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Application.Comments.Repositories
{
    public interface ICommentRepository
    {
        public Task AddAsync(Comment entity, CancellationToken cancellationToken);
        public Task UpdateAsync(Comment entity, CancellationToken cancellationToken);
        public Task<Comment> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] key);
        public Task<IQueryable<Comment>> GetAll(CancellationToken cancellationToken);
    }
}
