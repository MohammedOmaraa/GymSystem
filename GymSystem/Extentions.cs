using GymSystem.DAL.Contexts;
using GymSystem.DAL.DataSeeds;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem;

public static class Extensions
{
    public static async Task MigrateAndSeedAsync(
        this WebApplication app,
        CancellationToken ct = default)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<GymDbContext>();

        var logger = services.GetRequiredService<ILogger<Program>>();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var pendingMigrations =
            await context.Database.GetPendingMigrationsAsync(ct);

        if (pendingMigrations.Any())
        {
            logger.LogInformation(
                "Applying {Count} pending migrations...",
                pendingMigrations.Count());

            await context.Database.MigrateAsync(ct);

            logger.LogInformation(
                "Database migrations applied successfully.");
        }
        else
        {
            logger.LogInformation(
                "Database is already up to date.");
        }

        var seedFilesPath = Path.Combine(
            AppContext.BaseDirectory,
            "DataSeeds",
            "Files");

        await DataSeeder.SeedAsync(
            context,
            seedFilesPath,
            logger,
            ct);

        await IdentityDataSeeds.SeedIdentityAsync(
            roleManager,
            userManager,
            logger,
            ct);
    }
}