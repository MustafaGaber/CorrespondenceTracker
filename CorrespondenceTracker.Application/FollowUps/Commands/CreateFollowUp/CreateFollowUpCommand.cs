using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.FollowUps.Commands.CreateFollowUp
{
    public interface ICreateFollowUpCommand
    {
        Task<Guid> Execute(Guid correspondenceId, CreateFollowUpRequest request);
    }

    public class CreateFollowUpCommand : ICreateFollowUpCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateFollowUpCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(Guid correspondenceId, CreateFollowUpRequest request)
        {
            var correspondence = await _context.Correspondences.FindAsync(correspondenceId)
                ?? throw new ArgumentException($"Correspondence with ID {correspondenceId} not found");

            var followUp = new Domain.Entities.FollowUp(
                correspondenceId,
                request.UserId,
                request.Date,
                request.Details,
                request.FileRecordId
            );

            _context.FollowUps.Add(followUp);
            await _context.SaveChangesAsync();

            return followUp.Id;
        }
    }
}