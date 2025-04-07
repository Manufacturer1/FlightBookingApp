using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void DeleteFile(string fileNameWithExtension)
        {
             if(string.IsNullOrEmpty(fileNameWithExtension)) 
                    throw new ArgumentNullException(nameof(fileNameWithExtension));

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

            if (!File.Exists(path))
                throw new FileNotFoundException("Invalid file path");

            File.Delete(path);
        }

        public async Task<string> SaveFileAsync(IFormFile? imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
                throw new ArgumentNullException(nameof(imageFile));

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            Directory.CreateDirectory(path); 

            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !allowedFileExtensions.Contains(ext))
                throw new ArgumentException($"Only {string.Join(", ", allowedFileExtensions)} are allowed");

            byte[] incomingHash;
            await using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                incomingHash = ComputeHash(ms);
            }

            foreach (var existingFile in Directory.GetFiles(path))
            {
                if (FilesAreEqual(await File.ReadAllBytesAsync(existingFile), incomingHash))
                {
                    return Path.GetFileName(existingFile); 
                }
            }

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(imageFile.FileName)}_{Guid.NewGuid().ToString()[..8]}{ext}";
            var fullPath = Path.Combine(path, uniqueFileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        private bool FilesAreEqual(byte[] fileBytes, byte[] incomingHash)
        {
            return ComputeHash(new MemoryStream(fileBytes)).SequenceEqual(incomingHash);
        }

        private byte[] ComputeHash(Stream stream)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            stream.Position = 0; 
            return sha256.ComputeHash(stream);
        }
    }
}
