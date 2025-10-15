using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondence
{
    public class CreateCorrespondenceCommand : ICreateCorrespondenceCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateCorrespondenceCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateCorrespondenceRequest model)
        {
            var correspondent = await _context.Correspondents.FindAsync(model.SenderId)
                ?? throw new ArgumentException($"Correspondent with ID {model.SenderId} not found");

            if (model.DepartmentId.HasValue &&
                await _context.Departments.FindAsync(model.DepartmentId.Value) == null)
                throw new ArgumentException($"Department with ID {model.DepartmentId} not found");

            if (model.AssignedUserId.HasValue &&
                await _context.Users.FindAsync(model.AssignedUserId.Value) == null)
                throw new ArgumentException($"User with ID {model.AssignedUserId} not found");

            Subject? subject = null;
            if (model.SubjectId.HasValue)
            {
                subject = await _context.Subjects.FindAsync(model.SubjectId.Value)
                    ?? throw new ArgumentException($"Subject with ID {model.SubjectId} not found");
            }

            List<Classification>? classifications = null;
            if (model.ClassificationIds?.Any() == true)
            {
                classifications = await _context.Classifications
                    .Where(c => model.ClassificationIds.Contains(c.Id))
                    .ToListAsync();
            }

            var correspondence = new Correspondence(
                direction: model.Direction ?? CorrespondenceDirection.Incoming,
                priorityLevel: PriorityLevel.Medium,
                incomingNumber: model.IncomingNumber,
                incomingDate: model.IncomingDate,
                correspondentId: model.SenderId,
                outgoingNumber: model.OutgoingNumber,
                outgoingDate: model.OutgoingDate,
                departmentId: model.DepartmentId,
                content: model.Content,
                summary: model.Summary,
                assignedUserId: model.AssignedUserId,
                notes: model.Notes,
                mainFileId: null,
                isClosed: model.IsClosed,
                subjectId: model.SubjectId,
                classifications: classifications
            );

            _context.Correspondences.Add(correspondence);
            await _context.SaveChangesAsync();
            return correspondence.Id;
        }

    }

    public interface ICreateCorrespondenceCommand
    {
        Task<Guid> Execute(CreateCorrespondenceRequest model);
    }
}