using CorrespondenceTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IFileService
    {
        void DeleteFile(string path, string userId);
        Task<FileData> UploadFile(IFormFile file);
        Task<MemoryStream> ReadFile(string path);
        void DeleteFile(string path);
    }
}
