



namespace Ordering.Infratstructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            // Register infrastructure services here
            var connectionString = configuration.GetConnectionString("Database");
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.AddInterceptors(new AuditableEntityInterceptor());
                    options.UseSqlServer(connectionString);
                });
               // services.AddScoped<IOrderRepository, ApplicationDbContext>();
            return services;
        }
    }
}
