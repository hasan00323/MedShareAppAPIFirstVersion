using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileService.Interface
{
    public interface IFileStorageService
    {
        Task<List<string>> UploadAsync(
            List<IFormFile> files,
            string folderName,
            int minFiles,
            int maxFiles
        );
    }


}
