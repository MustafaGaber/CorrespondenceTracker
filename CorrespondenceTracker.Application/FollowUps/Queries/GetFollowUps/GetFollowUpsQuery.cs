using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.FollowUps.Queries.GetFollowUps
{
    public interface IGetFollowUpsQuery
    {
        Task<List<GetFollowUpResponse>> Execute(Guid correspondenceId);
    }

    public class GetFollowUpsQuery : IGetFollowUpsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetFollowUpsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<GetFollowUpResponse>> Execute(Guid correspondenceId)
        {
            var followUps = await _context.FollowUps
                .Include(f => f.User)
                .Include(f => f.FileRecord)
                .Where(f => f.CorrespondenceId == correspondenceId)
                .OrderByDescending(f => f.Date)
                .ToListAsync();

            return followUps.Select(f => new GetFollowUpResponse
            {
                Id = f.Id,
                CorrespondenceId = f.CorrespondenceId,
                UserId = f.UserId,
                UserName = f.User?.FullName,
                Date = f.Date,
                Details = f.Details,
                FileRecordId = f.FileRecordId,
                FileName = f.FileRecord?.FileName
            }).ToList();
        }
    }
}