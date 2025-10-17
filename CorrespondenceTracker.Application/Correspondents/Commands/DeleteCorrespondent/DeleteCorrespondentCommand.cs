// file: CorrespondenceTracker.Application/Correspondents/Commands/DeleteCorrespondent/DeleteCorrespondentCommand.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondents.Commands.DeleteCorrespondent
{
    public class DeleteCorrespondentCommand : IDeleteCorrespondentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteCorrespondentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var correspondent = await _context.Correspondents.FindAsync(id)
                ?? throw new ArgumentException($"Correspondent with ID {id} not found");

            // Check if correspondent is referenced by any correspondence
            var hasReferences = await _context.Correspondences
                .AnyAsync(c => c.CorrespondentId == id);

            if (hasReferences)
            {
                throw new InvalidOperationException("Cannot delete correspondent as it is referenced by one or more correspondences");
            }

            _context.Correspondents.Remove(correspondent);
            await _context.SaveChangesAsync();
        }
    }

    public interface IDeleteCorrespondentCommand
    {
        Task Execute(Guid id);
    }
}