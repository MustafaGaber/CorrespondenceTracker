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

        public async Task Execute(Guid id, UpdateCorrespondenceRequest model)
        {
            var correspondence = await _context.Correspondences
                .Include(c => c.Classifications)
                .Include(c => c.Reminders)
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
                FileData fileData = await _fileService.UploadFile(model.File);
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

            // Handle reminders intelligently
            UpdateReminders(correspondence, model.Reminders);

            correspondence.Update(
                model.IncomingNumber,
                model.IncomingDate,
                model.OutgoingNumber,
                model.OutgoingDate,
                model.DepartmentId,
                model.Content,
                model.Summary,
                model.FollowUpUserId,
                model.ResponsibleUserId,
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

        private void UpdateReminders(Correspondence correspondence, List<ReminderDto>? reminderDtos)
        {
            var existingReminders = correspondence.Reminders.ToList();

            if (reminderDtos == null || !reminderDtos.Any())
            {
                // No reminders in request - delete all existing
                foreach (var existing in existingReminders)
                {
                    _context.Reminders.Remove(existing);
                }
                return;
            }

            // Track which existing reminder IDs are being updated
            var processedReminderIds = new HashSet<Guid>();

            // Process DTOs: update existing or add new
            foreach (var reminderDto in reminderDtos)
            {
                if (reminderDto.Id.HasValue)
                {
                    // Update existing reminder
                    var existing = existingReminders.FirstOrDefault(r => r.Id == reminderDto.Id.Value);

                    if (existing == null)
                        throw new ArgumentException($"Reminder with ID {reminderDto.Id.Value} not found");

                    existing.Update(
                        reminderDto.RemindTime,
                        reminderDto.SendEmailMessage,
                        reminderDto.Message
                    );
                    processedReminderIds.Add(existing.Id);
                }
                else
                {
                    // Add new reminder (Id is null)
                    var newReminder = new Reminder(
                        correspondenceId: correspondence.Id,
                        remindTime: reminderDto.RemindTime,
                        sendEmailMessage: reminderDto.SendEmailMessage,
                        message: reminderDto.Message
                    );
                    _context.Reminders.Add(newReminder);
                }
            }

            // Delete reminders that weren't in the request
            foreach (var existing in existingReminders)
            {
                if (!processedReminderIds.Contains(existing.Id))
                {
                    _context.Reminders.Remove(existing);
                }
            }
        }
    }
}