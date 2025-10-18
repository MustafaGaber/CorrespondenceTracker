using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Classifications.Commands.UpdateClassification
{
    public interface IUpdateClassificationCommand
    {
        Task Execute(Guid id, UpdateClassificationRequest request);
    }

    public class UpdateClassificationCommand : IUpdateClassificationCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateClassificationCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, UpdateClassificationRequest request)
        {
            var classification = await _context.Classifications.FindAsync(id)
                ?? throw new ArgumentException($"Classification with ID {id} not found");

            classification.Update(request.Name);

            await _context.SaveChangesAsync();
        }
    }
}