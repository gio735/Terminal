using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Managers;
using Terminal.Domain;
using Terminal.Domain.Models;

namespace Terminal.Persistence.Data.Seed
{
    public static class DataSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DataContext>();
            Migrate(database);
            SeedData(database);
            LoadData(database);
        }
        private static void Migrate(DataContext context)
        {
            context.Database.Migrate();
        }

        private static void SeedData(DataContext context)
        {
            PasswordHasher<User> passwordHasher = new();
            List<User> staffTeam = new()
            {
                new User
                {
                    UserName = "Nervozaaa",
                    Email = "m.vakhtangishvili@sangu.edu.ge",
                    Password = "mtvralikaci123",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "N1KA_B",
                    Email = "n.buzaladze1@sangu.edu.ge",
                    Password = "successIsInTwoSteps",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "Somala",
                    Email = "d.eliashvili@sangu.edu.ge",
                    Password = "zahesiIsReal123",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "Ravelcut",
                    Email = "l.bezhashvili@sangu.edu.ge",
                    Password = "coconutdogloverboy1000",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "DGRAPH",
                    Email = "d.tchutchulashvili@sangu.edu.ge",
                    Password = "DaGraphIsMehh",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "Eliseus",
                    Email = "e.bazgadze@sangu.edu.ge",
                    Password = "pogchampegirlhunter",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                },
                new User
                {
                    UserName = "ILHYSM",
                    Email = "g.turashvili@sangu.edu.ge",
                    Password = "mtvralikaci123",
                    UserRank = Rank.Owner,
                    UserStatus = Status.Management
                }
            };
            
            foreach (var user in staffTeam)
            {
                if (context.Users.Any(e => e.Email == user.Email || e.UserName == user.UserName))
                {
                    continue;
                }
                user.Password = passwordHasher.HashPassword(null, user.Password);
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

        private static void LoadData(DataContext context)
        {
            ManagerService.References = context.References.ToList();
        }

    }
}
