﻿using Microsoft.EntityFrameworkCore;
using Terminal.Application.Comments.Repositories;
using Terminal.Application.Definitions.Repositories;
using Terminal.Application.References.Repositories;
using Terminal.Application.Reports.Repositories;
using Terminal.Application.Users;
using Terminal.Application.Users.Repositories;
using Terminal.Application.Votes.Repositories;
using Terminal.Infrastructure.Repositories;
using Terminal.Persistence.Data;

namespace Terminal.MVC.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IDefinitionRepository, DefinitionRepository>();
            services.AddScoped<IReferenceRepository, ReferenceRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();


            services.AddScoped<IUserService, UserService>();

            services.AddScoped<DbContext, DataContext>();
            return services;
        }
    }
}
