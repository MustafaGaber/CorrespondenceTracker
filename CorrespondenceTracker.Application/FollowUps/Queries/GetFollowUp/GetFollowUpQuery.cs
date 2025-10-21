using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.FollowUps.Queries.GetFollowUp
{
    public interface IGetFollowUpQuery
    {
        Task<GetFollowUpDetailResponse> Execute(Guid id);
    }

    public class GetFollowUpQuery : IGetFollowUpQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetFollowUpQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<GetFollowUpDetailResponse> Execute(Guid id)
        {
            var followUp = await _context.FollowUps
                .Include(f => f.User)
                .Include(f => f.FileRecord)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null)
            {
                throw new ArgumentException($"Follow-up with ID {id} not found");
            }

            return new GetFollowUpDetailResponse
            {
                Id = followUp.Id,
                CorrespondenceId = followUp.CorrespondenceId,
                UserId = followUp.UserId,
                UserName = followUp.User?.FullName,
                Date = followUp.Date,
                Details = followUp.Details,
                FileRecordId = followUp.FileRecordId,
                FileName = followUp.FileRecord?.FileName
            };
        }
    }
}