using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

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
            // Validate required relationships
            var correspondent = await _context.Correspondents.FindAsync(model.SenderId);
            if (correspondent == null)
                throw new ArgumentException($"Correspondent with ID {model.SenderId} not found");

            // Validate optional relationships
            if (model.DepartmentId.HasValue)
            {
                var department = await _context.Departments.FindAsync(model.DepartmentId.Value);
                if (department == null)
                    throw new ArgumentException($"Department with ID {model.DepartmentId} not found");
            }

            if (model.AssignedUserId.HasValue)
            {
                var assignedUser = await _context.Users.FindAsync(model.AssignedUserId.Value);
                if (assignedUser == null)
                    throw new ArgumentException($"User with ID {model.AssignedUserId} not found");
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
                isClosed: model.IsClosed
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