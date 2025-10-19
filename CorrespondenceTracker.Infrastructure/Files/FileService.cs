using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CorrespondenceTracker.Infrastructure.Files
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileService> _logger;
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions;
        private readonly string _storagePath;
        private readonly string _trashPath;

        public FileService(IConfiguration configuration, ILogger<FileService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _maxFileSize = 104857600; // 100MB default
            _allowedExtensions = ["jpg", "jpeg", "png", "pdf", "dwg", "xls", "xlsx"];
            _storagePath = _configuration["Storage:Path"] ??
                throw new InvalidOperationException("Storage:Path configuration is required");
            _trashPath = Path.Combine(_storagePath, "Trash");
            EnsureDirectoryExists(_trashPath);
        }

        public async Task<FileData> UploadFile(IFormFile file, string destinationFolderPath)
        {
            ValidateFile(file);
            ValidatePath(destinationFolderPath);
            EnsureDirectoryExists(destinationFolderPath);
            string uniqueFileName = GenerateUniqueFileName(file, destinationFolderPath);
            string filePath = Path.Combine(destinationFolderPath, uniqueFileName);
            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);
                return new FileData
                {
                    FullPath = filePath,
                    Extension = Path.GetExtension(file.FileName).ToLower().TrimStart('.'),
                    Size = file.Length
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName} to {Path}", file.FileName, destinationFolderPath);
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); }
                    catch (Exception deleteEx) { _logger.LogError(deleteEx, "Failed to delete partial file: {FilePath}", filePath); }
                }
                throw;
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteFile(string path, string userId)
        {
            ValidatePath(path);
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            try
            {
                string userTrashPath = Path.Combine(_trashPath, userId);
                EnsureDirectoryExists(userTrashPath);
                string relativePath = path.Substring(_storagePath.Length).TrimStart(Path.DirectorySeparatorChar);
                string trashFilePath = Path.Combine(userTrashPath, relativePath);
                string trashFileDir = Path.GetDirectoryName(trashFilePath);
                if (!string.IsNullOrEmpty(trashFileDir))
                {
                    EnsureDirectoryExists(trashFileDir);
                }
                trashFilePath = GetUniqueFilePath(trashFilePath);
                File.Move(path, trashFilePath);
                _logger.LogInformation("File moved to trash: {FilePath} -> {TrashPath}", path, trashFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to move file to trash: {FilePath}", path);
                throw;
            }
        }

        private void MergeFolders(string sourcePath, string targetPath)
        {
            // Ensure target directory exists
            EnsureDirectoryExists(targetPath);

            // Move all files from source to target
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetPath, fileName);
                destFile = GetUniqueFilePath(destFile);
                File.Move(file, destFile);
            }

            // Recursively merge subdirectories
            foreach (string dir in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(dir);
                string destDir = Path.Combine(targetPath, dirName);
                MergeFolders(dir, destDir);
            }
        }

        private string GetUniqueFilePath(string path)
        {
            if (!File.Exists(path))
                return path;

            string directory = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;

            string newPath;
            do
            {
                string newFileName = $"{fileNameWithoutExtension}_{counter}{extension}";
                newPath = Path.Combine(directory, newFileName);
                counter++;
            } while (File.Exists(newPath));

            return newPath;
        }

        public async Task<MemoryStream> ReadFile(string path)
        {
            ValidatePath(path);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            try
            {
                var memoryStream = new MemoryStream();
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                _logger.LogInformation("File read successfully: {FilePath}", path);
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read file: {FilePath}", path);
                throw;
            }
        }

        public void DeleteFile(string path)
        {
            ValidatePath(path);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            try
            {
                File.Delete(path);
                _logger.LogInformation("File deleted successfully: {FilePath}", path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file: {FilePath}", path);
                throw;
            }
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required and cannot be empty.");
            }

            string extension = Path.GetExtension(file.FileName).ToLower().TrimStart('.');
            if (!_allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Unsupported file type '{extension}'. Allowed types: {string.Join(", ", _allowedExtensions)}");
            }

            if (file.Length > _maxFileSize)
            {
                throw new InvalidOperationException($"File size exceeds limit. Maximum allowed size is {_maxFileSize / (1024 * 1024)}MB.");
            }
        }

        private void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            }
            /*string normalizedPath = Path.GetFullPath(path);
            string normalizedStoragePath = Path.GetFullPath(_storagePath);
            if (!normalizedPath.StartsWith(normalizedStoragePath, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Access to path outside storage directory is not allowed.");
            }*/
        }

        private string SanitizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", path.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }

        private string GenerateUniqueFileName(IFormFile file, string folder)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName;

            do
            {
                string randomName = Path.GetRandomFileName().Replace('.', '_');
                fileName = $"{randomName}{extension}";
            }
            while (File.Exists(Path.Combine(folder, fileName)));
            return fileName;
        }

    }
}
