using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Terminal.Domain;
using Terminal.Domain.Utilities;
using System.Diagnostics.Metrics;
using System.Net;
using Terminal.Domain.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace Terminal.Persistence.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.UserId, v.DefinitionId });
            modelBuilder.Entity<Vote>()
                .Property(e => e.Likes).IsRequired();

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityModel)entityEntry.Entity).ModifiedDate = DateTime.UtcNow.AddHours(4);

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityModel)entityEntry.Entity).CreationDate = DateTime.UtcNow.AddHours(4);
                }
            }

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
            .Entries()
                .Where(e => e.Entity is EntityModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityModel)entityEntry.Entity).ModifiedDate = DateTime.UtcNow.AddHours(4);

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityModel)entityEntry.Entity).CreationDate = DateTime.UtcNow.AddHours(4);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BAGIRSC;Database=TerminalDEV;Trusted_Connection=True;TrustServerCertificate=True;");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
