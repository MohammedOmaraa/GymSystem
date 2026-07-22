using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymSystem.BLL.Services.Classes;

public class AttachmentServices : IAttachmentServices
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<AttachmentServices> logger;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png"
    ];

    private static readonly Dictionary<string, string> ContentTypes =
        new()
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png"
        };

    public AttachmentServices(
        IWebHostEnvironment env,
        ILogger<AttachmentServices> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    private string GetFolderPath(string folderName)
    {
        return Path.Combine(env.ContentRootPath, folderName);
    }

    public async Task<Result<string>> UploadAsync(
        Stream stream,
        string fileName,
        string folderName,
        CancellationToken ct = default)
    {
        if (stream == null || !stream.CanRead || stream.Length == 0)
            return Result<string>.Validation("Invalid file.");

        if (stream.Length > MaxFileSize)
            return Result<string>.Validation("File exceeds maximum size.");

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            return Result<string>.Validation("Unsupported file type.");

        var folder = GetFolderPath(folderName);

        Directory.CreateDirectory(folder);

        var storedFileName = $"{Guid.NewGuid():N}{extension}";

        var fullPath = Path.Combine(folder, storedFileName);

        try
        {
            await using var fs = new FileStream(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None);

            await stream.CopyToAsync(fs, ct);

            return Result<string>.Success(storedFileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error uploading file {FileName}",
                fileName);

            return Result<string>.Failure("Unable to upload file.");
        }
    }

    public Result Delete(
        string fileName,
        string folderName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Validation("Invalid file name.");

        var fullPath = Path.Combine(
            GetFolderPath(folderName),
            fileName);

        try
        {
            if (!File.Exists(fullPath))
                return Result.NotFound("File not found.");

            File.Delete(fullPath);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error deleting file {FileName}",
                fileName);

            return Result.Failure("Unable to delete file.");
        }
    }

    public Result<(Stream Stream, string ContentType)> GetFile(
        string fileName,
        string folderName)
    {
        var fullPath = Path.Combine(
            GetFolderPath(folderName),
            fileName);

        if (!File.Exists(fullPath))
            return Result<(Stream, string)>
                .NotFound("File not found.");

        var extension = Path.GetExtension(fullPath);

        if (!ContentTypes.TryGetValue(
            extension,
            out var contentType))
        {
            contentType = "application/octet-stream";
        }

        Stream stream = File.OpenRead(fullPath);

        return Result<(Stream, string)>
            .Success((stream, contentType));
    }
}