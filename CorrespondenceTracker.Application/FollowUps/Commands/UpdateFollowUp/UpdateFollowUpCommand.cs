using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.FollowUps.Commands.UpdateFollowUp
{
    public interface IUpdateFollowUpCommand
    {
        Task Execute(Guid id, UpdateFollowUpRequest request);
    }

    public class UpdateFollowUpCommand : IUpdateFollowUpCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateFollowUpCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, UpdateFollowUpRequest request)
        {
            var followUp = await _context.FollowUps.FindAsync(id)
                ?? throw new ArgumentException($"FollowUp with ID {id} not found");

            followUp.Update(
                request.UserId,
                request.Date,
                request.Details,
                request.FileRecordId
            );

            await _context.SaveChangesAsync();
        }
    }
}