using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymSystem.DAL.DataSeeds;

internal static class JsonSeeder
{
    public static async Task<List<T>> LoadAsync<T>(
        string folderPath,
        string fileName,
        ILogger logger)
    {
        var filePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(filePath))
        {
            logger.LogError(
                "Seed file not found: {Path}",
                filePath);

            throw new FileNotFoundException(filePath);
        }

        await using var stream = File.OpenRead(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return await JsonSerializer.DeserializeAsync<List<T>>
        (
            stream,
            options
        ) ?? [];
    }
}