using Infrastructure.FileService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


namespace Infrastructure.FileService.Implementation
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IHostEnvironment _env;

        public FileStorageService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> UploadAsync(
            List<IFormFile> files,
            string folderName,
            int minFiles,
            int maxFiles)
        {
            if (files == null || files.Count < minFiles)
                throw new Exception($"Minimum {minFiles} image(s) required");

            if (files.Count > maxFiles)
                throw new Exception($"Maximum {maxFiles} images allowed");

            var rootPath = Path.Combine(
                _env.ContentRootPath,
                "Uploads",
                folderName
            );

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var paths = new List<string>();

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(rootPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                paths.Add($"/uploads/{folderName}/{fileName}");
            }

            return paths;
        }
    }

}
