using Microsoft.Extensions.WebEncoders;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Terminal.MVC.Infrastructure.Auth.JWT;
using Terminal.Persistence.Data;
using Terminal.MVC.Infrastructure.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Terminal.Application.Helpers;
using Terminal.MVC.Infrastructure.Middlewares;
using System.Net.Sockets;
using Terminal.Persistence.Data.Seed;

namespace Terminal.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            builder.Services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(
                    UnicodeRanges.All);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddCookieAuthentication(builder.Configuration.GetSection(nameof(JWTConfiguration)).GetSection(nameof(JWTConfiguration.Secret)).Value);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddServices();
            builder.Services.AddMvc();

            string connectionString;
            if (builder.Environment.IsDevelopment())
            {
                connectionString = builder.Configuration.GetConnectionString("DevelopmentConnection");
            }
            else
            {
                connectionString = builder.Configuration.GetConnectionString("ReleaseConnection");
            }
            builder.Services.AddDbContext(connectionString!);

            builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWTConfiguration"))
            .Configure<SMTPConfiguration>(builder.Configuration.GetSection("SMTPConfiguration"));

            var app = builder.Build();

            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            DataSeed.Initialize(app.Services);
            app.Run();
        }
    }
}
