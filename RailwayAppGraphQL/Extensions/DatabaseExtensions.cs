using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Data.Seeders;

namespace RailwayAppGraphQL.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    ///     Applies any pending Entity Framework Core migrations to the database.
    ///     Creates a service scope to resolve the <see cref="ApplicationDbContext" /> and runs <c>Database.MigrateAsync</c>.
    ///     Logs success or any errors that occur during migration.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" /> used to create the service scope and resolve services.</param>
    /// <returns>A task representing the asynchronous migration operation.</returns>
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        var factory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        await using var db = await factory.CreateDbContextAsync();

        try
        {
            await db.Database.MigrateAsync();

            app.Logger.LogInformation("Database migration applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migration.");
            throw;
        }
    }

    /// <summary>
    ///     Seeds the database with initial data.
    ///     Creates a service scope to resolve the <see cref="ApplicationDbContext" /> and runs <c>DatabaseSeeder.SeedAsync</c>.
    ///     Logs success or any errors that occur during seeding.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" /> used to create the service scope and resolve services.</param>
    /// <returns>A task representing the asynchronous seeding operation.</returns>
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        var factory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        await using var db = await factory.CreateDbContextAsync();

        try
        {
            app.Logger.LogInformation("Starting database seeding...");

            await DatabaseSeeder.SeedAsync(db);

            app.Logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred during database seeding.");
            throw;
        }
    }
}