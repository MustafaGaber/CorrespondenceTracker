using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Commands.UpdateCorrespondence
{
    public class UpdateCorrespondenceCommand : IUpdateCorrespondenceCommand
    {
        private readonly CorrespondenceDatabaseContext _context;
        private readonly IFileService _fileService;

        public UpdateCorrespondenceCommand(CorrespondenceDatabaseContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task Execute(Guid id, CreateCorrespondenceRequest model)
        {
            var correspondence = await _context.Correspondences
                .Include(c => c.Classifications)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (correspondence == null)
                throw new ArgumentException($"Correspondence with ID {id} not found");

            List<Classification>? classifications = null;
            if (model.ClassificationIds?.Any() == true)
            {
                classifications = await _context.Classifications
                    .Where(c => model.ClassificationIds.Contains(c.Id))
                    .ToListAsync();

                if (classifications.Count != model.ClassificationIds.Count)
                {
                    var foundIds = classifications.Select(c => c.Id).ToList();
                    var missingIds = model.ClassificationIds.Except(foundIds).ToList();
                    throw new ArgumentException($"Classifications with IDs {string.Join(", ", missingIds)} not found");
                }
            }

            Guid? fileId = null;
            if (model.File != null)
            {
                FileData fileData = await _fileService.UploadFile(
                   model.File);
                FileRecord record = new FileRecord(
                    fileName: "",
                    fullPath: fileData.FullPath,
                    contentType: model.File.ContentType,
                    extension: fileData.Extension,
                    size: fileData.Size
                );
                await _context.FileRecords.AddAsync(record);
                fileId = record.Id;
            }

            correspondence.Update(
                model.IncomingNumber,
                model.IncomingDate,
                model.OutgoingNumber,
                model.OutgoingDate,
                model.DepartmentId,
                model.Content,
                model.Summary,
                model.AssignedUserId,
                model.Notes,
                fileId,
                model.IsClosed,
                model.SubjectId,
                classifications,
                model.Direction,
                model.PriorityLevel
            );

            await _context.SaveChangesAsync();
        }
    }


}
