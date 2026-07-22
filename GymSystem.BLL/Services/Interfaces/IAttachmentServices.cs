
using GymSystem.BLL.Common;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAttachmentServices
    {
        Task<Result<string>> UploadAsync(
        Stream stream,
        string fileName,
        string folderName,
        CancellationToken ct = default);

        Result Delete(
            string fileName,
            string folderName);

        Result<(Stream Stream, string ContentType)> GetFile(
            string fileName,
            string folderName);
    }
}
