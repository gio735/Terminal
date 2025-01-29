using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.References.Repositories;
using Terminal.Domain.Models;

namespace Terminal.Infrastructure.Repositories
{
    public class ReferenceRepository : RepositoryBase<Reference>, IReferenceRepository
    {
        public ReferenceRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
