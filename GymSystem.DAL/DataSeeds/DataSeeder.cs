using GymSystem.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymSystem.DAL.DataSeeds;

public static class DataSeeder
{
    public static async Task SeedAsync(
        GymDbContext context,
        string seedFolder,
        ILogger logger,
        CancellationToken ct = default)
    {
        try
        {
            var total = 0;

            total += await SeedEntityAsync(
                context.Plans,
                seedFolder,
                "Plans.json",
                logger,
                ct);

            total += await SeedEntityAsync(
                context.Trainers,
                seedFolder,
                "Trainers.json",
                logger,
                ct);            

            if (!context.ChangeTracker.HasChanges())
            {
                logger.LogInformation(
                    "Database already contains seed data.");

                return;
            }

            await context.SaveChangesAsync(ct);

            logger.LogInformation(
                "Database seeded successfully. {Count} records inserted.",
                total);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while seeding database.");

            throw;
        }
    }

    private static async Task<int> SeedEntityAsync<TEntity>(
        DbSet<TEntity> dbSet,
        string folderPath,
        string fileName,
        ILogger logger,
        CancellationToken ct)
        where TEntity : class
    {
        if (await dbSet.AnyAsync(ct))
        {
            logger.LogInformation(
                "{Entity} already exists. Skipping.",
                typeof(TEntity).Name);

            return 0;
        }

        var data = await JsonSeeder.LoadAsync<TEntity>(
            folderPath,
            fileName,
            logger);

        if (data.Count == 0)
        {
            logger.LogWarning(
                "{File} is empty.",
                fileName);

            return 0;
        }

        await dbSet.AddRangeAsync(data, ct);

        logger.LogInformation(
            "{Count} {Entity} loaded.",
            data.Count,
            typeof(TEntity).Name);

        return data.Count;
    }
}