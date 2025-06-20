﻿using Microsoft.AspNetCore.Http;

namespace ServerLibrary.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile? imageFile, string[] allowedFileExtensions);
        void DeleteFile(string fileNameWithExtension);
    }
}
