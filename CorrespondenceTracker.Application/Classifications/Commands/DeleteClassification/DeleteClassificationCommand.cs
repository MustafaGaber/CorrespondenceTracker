using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Classifications.Commands.DeleteClassification
{
    public interface IDeleteClassificationCommand
    {
        Task Execute(Guid id);
    }

    public class DeleteClassificationCommand : IDeleteClassificationCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteClassificationCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var classification = await _context.Classifications.FindAsync(id)
                ?? throw new ArgumentException($"Classification with ID {id} not found");

            _context.Classifications.Remove(classification);
            await _context.SaveChangesAsync();
        }
    }
}