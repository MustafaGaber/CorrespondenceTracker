using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.FollowUps.Commands.DeleteFollowUp
{
    public interface IDeleteFollowUpCommand
    {
        Task Execute(Guid id);
    }

    public class DeleteFollowUpCommand : IDeleteFollowUpCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteFollowUpCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var followUp = await _context.FollowUps.FindAsync(id)
                ?? throw new ArgumentException($"FollowUp with ID {id} not found");

            _context.FollowUps.Remove(followUp);
            await _context.SaveChangesAsync();
        }
    }
}