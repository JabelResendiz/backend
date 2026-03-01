
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Finlay.PharmaVigilance.Infrastructure.Initializer;

/// <summary>
/// Responsible for initializing the database by applying migrations automatically.
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Applies database migrations to ensure the database schema is up-to-date with the application's data model.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve required services, such as the FinlayDbContext.</param>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {

        // Create a new scope to resolve services with a scoped lifecycle.
        using var scope = serviceProvider.CreateScope();

        // Retrieve the database context from the scoped service provider.
        var context = scope.ServiceProvider.GetRequiredService<FinlayDbContext>();

        try
        {
            // First, ensure the database exists
            Console.WriteLine("Ensuring database exists...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✓ Database ensured.");

            // Then apply pending migrations
            Console.WriteLine("Applying migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("✓ Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Database initialization error: {ex.Message}");
            throw;
        }
    }

}
