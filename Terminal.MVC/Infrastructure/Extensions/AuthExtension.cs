using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;

namespace Terminal.MVC.Infrastructure.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, string key)
        {
            var keybytes = Encoding.ASCII.GetBytes(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(x =>
            {
                x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                x.Cookie.HttpOnly = true;
                x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                x.AccessDeniedPath = "/Forbidden/";
                x.LoginPath = "/Auth/SignIn";
            });

            return services;
        }
    }
}
