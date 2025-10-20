using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Files.Queries.DownloadFile
{
    public class DownloadFileQuery : IDownloadFileQuery
    {
        private readonly CorrespondenceDatabaseContext _context;
        private readonly IFileService _fileService;

        public DownloadFileQuery(CorrespondenceDatabaseContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<DownloadFileResponse> Execute(Guid fileId)
        {
            var file = await _context.FileRecords
                .Select(d => new { d.Id, d.FullPath, d.FileName })
                .FirstOrDefaultAsync(d => d.Id == fileId) ?? throw new ArgumentException("Document not found");

            var stream = await _fileService.ReadFile(file.FullPath);
            return new DownloadFileResponse
            {
                Stream = stream,
                Name = file.FileName,
            };
        }
    }
}
