using CorrespondenceTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IFileService
    {
        void DeleteFile(string path, string userId);
        Task<FileData> UploadDocument(IFormFile file, string folderName);
        Task<MemoryStream> ReadFile(string path);
        void DeleteFile(string path);
    }
}
