using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Terminal.Persistence.Data
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
      services.AddDbContext<DataContext>(options =>
          options.UseSqlServer(connectionString));
    }
}
