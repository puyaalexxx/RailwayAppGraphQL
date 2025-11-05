using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;

namespace RailwayAppGraphQL.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Applies any pending Entity Framework Core migrations to the database.
    /// Creates a service scope to resolve the <see cref="ApplicationDbContext"/> and runs <c>Database.MigrateAsync</c>.
    /// Logs success or any errors that occur during migration.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> used to create the service scope and resolve services.</param>
    /// <returns>A task representing the asynchronous migration operation.</returns>
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await context.Database.MigrateAsync();

            app.Logger.LogInformation("Database migration applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migration.");

            throw;
        }

    }
}