using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Extensions
{
    public static class MigrationExtensions
    {
        public static WebApplication UseMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
            
            try
            {
                app.Logger.LogInformation("Starting database migration and validation...");
                app.Logger.LogInformation("Connection string: {ConnectionString}", context.Database.GetConnectionString());
                
                // Check if database exists and has pending migrations
                var pendingMigrations = context.Database.GetPendingMigrations();
                var appliedMigrations = context.Database.GetAppliedMigrations();
                
                app.Logger.LogInformation("Applied migrations: {AppliedCount}, Pending migrations: {PendingCount}", 
                    appliedMigrations.Count(), pendingMigrations.Count());
                
                // Only apply migrations if there are pending ones
                if (pendingMigrations.Any())
                {
                    app.Logger.LogInformation("Applying pending migrations...");
                    context.Database.Migrate();
                }
                else
                {
                    app.Logger.LogInformation("No pending migrations to apply");
                }
                
                // Validate that required tables exist by trying to query the Coupons table
                try
                {
                    var couponCount = context.Coupons.Count();
                    app.Logger.LogInformation("Database validation successful. Coupons table exists with {Count} records", couponCount);
                }
                catch (Exception tableEx)
                {
                    app.Logger.LogError(tableEx, "Coupons table does not exist or is not accessible");
                    throw new InvalidOperationException("Required database tables are missing");
                }
                
                app.Logger.LogInformation("Database migrations completed successfully");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while applying database migrations");
                throw;
            }
            
            return app;
        }
    }
}