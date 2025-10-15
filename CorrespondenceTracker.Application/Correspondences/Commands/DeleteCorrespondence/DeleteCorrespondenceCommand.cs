using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Commands.DeleteCorrespondence
{
    public class DeleteCorrespondenceCommand : IDeleteCorrespondenceCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteCorrespondenceCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var correspondence = await _context.Correspondences
                .FirstOrDefaultAsync(c => c.Id == id);

            if (correspondence == null)
                throw new ArgumentException($"Correspondence with ID {id} not found");

            _context.Correspondences.Remove(correspondence);
            await _context.SaveChangesAsync();
        }
    }


}
