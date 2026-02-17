
namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Register API services here
            services.AddCarter();
            return services;
        }

        public static WebApplication UseApiServices (this WebApplication app)
        {
            // Configure the HTTP request pipeline here
            app.MapCarter();
            return app;
        }
    }
}
