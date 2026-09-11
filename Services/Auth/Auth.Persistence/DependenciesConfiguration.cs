using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Persistence;

public static class DependenciesConfiguration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Database");
        services.AddDbContext<AuthDBContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}
