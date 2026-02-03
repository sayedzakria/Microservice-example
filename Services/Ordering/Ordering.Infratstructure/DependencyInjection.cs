using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Ordering.Infratstructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            // Register infrastructure services here
            var connectionString = configuration.GetConnectionString("Database");
            return services;
        }
    }
}
