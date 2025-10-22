// CreateCorrespondenceFromImageCommand.cs

using Ardalis.GuardClauses;
using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondenceFromImage
{
    public interface ICreateCorrespondenceFromImageCommand
    {
        Task<CreateCorrespondenceFromImageResponse> Execute(CreateCorrespondenceFromImageRequest model);
    }

    public class CreateCorrespondenceFromImageCommand : ICreateCorrespondenceFromImageCommand
    {
        private readonly CorrespondenceDatabaseContext _context;
        private readonly IFileService _fileService;
        private readonly IGeminiOcrService _ocrService;
        private readonly IGeminiSummarizerService _summarizerService;

        public CreateCorrespondenceFromImageCommand(
            CorrespondenceDatabaseContext context,
            IFileService fileService,
            IGeminiOcrService ocrService,
            IGeminiSummarizerService summarizerService)
        {
            _context = context;
            _fileService = fileService;
            _ocrService = ocrService;
            _summarizerService = summarizerService;
        }

        public async Task<CreateCorrespondenceFromImageResponse> Execute(CreateCorrespondenceFromImageRequest model)
        {
            // 1. Validate input
            Guard.Against.Null(model.File, nameof(model.File));
            Guard.Against.Default(model.SenderId, nameof(model.SenderId));

            var correspondent = await _context.Correspondents.FindAsync(model.SenderId)
                ?? throw new ArgumentException($"Correspondent with ID {model.SenderId} not found");

            // 2. Save the file temporarily to get its path
            FileData fileData = await _fileService.UploadFile(model.File);

            // 3. Use Gemini services
            string extractedText = await _ocrService.ExtractTextFromFileAsync(fileData.FullPath);
            string summarizedText = await _summarizerService.SummarizeTextAsync(extractedText);

            // 4. Create a permanent file record
            var fileRecord = new FileRecord(
                fileName: model.File.FileName,
                fullPath: fileData.FullPath,
                contentType: model.File.ContentType,
                extension: fileData.Extension,
                size: fileData.Size
            );
            await _context.FileRecords.AddAsync(fileRecord);

            // 5. Create the new correspondence entity with default values
            var correspondence = new Correspondence(
                direction: CorrespondenceDirection.Incoming,
                priorityLevel: PriorityLevel.Medium,
                correspondentId: model.SenderId,
                content: extractedText,
                summary: summarizedText,
                fileId: fileRecord.Id
            );

            _context.Correspondences.Add(correspondence);
            await _context.SaveChangesAsync();

            // 6. Return the response object
            return new CreateCorrespondenceFromImageResponse
            {
                Id = correspondence.Id,
                Content = correspondence.Content ?? string.Empty,
                Summary = correspondence.Summary ?? string.Empty,
                FileId = correspondence.FileId.Value
            };
        }
    }
}