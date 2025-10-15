using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Commands.UpdateCorrespondence
{
    public class UpdateCorrespondenceCommand : IUpdateCorrespondenceCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateCorrespondenceCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, CreateCorrespondenceRequest model)
        {
            var correspondence = await _context.Correspondences
                .Include(c => c.Classifications)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (correspondence == null)
                throw new ArgumentException($"Correspondence with ID {id} not found");

            // Update scalar properties
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
                model.IsClosed,
                model.SubjectId,
                [] // classifications
            );

            await _context.SaveChangesAsync();
        }
    }


}
