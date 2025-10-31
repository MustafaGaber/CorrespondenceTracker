using CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences;
using CorrespondenceTracker.Application.Reminders.Queries.GetReminders;
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetDashboardData
{
    public interface IGetDashboardDataQuery
    {
        Task<GetDashboardDataResponse> Execute();
    }

    public class GetDashboardDataQuery : IGetDashboardDataQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetDashboardDataQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<GetDashboardDataResponse> Execute()
        {
            var now = DateTime.Now;
            var today = now.Date;

            // Execute queries in series instead of parallel
            var totalCount = await _context.Correspondences.AsNoTracking().CountAsync();
            var openCount = await _context.Correspondences.AsNoTracking().CountAsync(c => !c.IsClosed);

            var top10OpenCorrespondences = await _context.Correspondences
                .AsNoTracking()
                .Where(c => !c.IsClosed)
                .OrderBy(c => c.PriorityLevel)
                .ThenBy(c => c.IncomingDate)
                .Take(10)
                .Select(c => new GetCorrespondenceItemResponse
                {
                    Id = c.Id,
                    Direction = c.Direction,
                    PriorityLevel = c.PriorityLevel,
                    IncomingNumber = c.IncomingNumber ?? string.Empty,
                    IncomingDate = c.IncomingDate.HasValue ? c.IncomingDate.Value : default,
                    Correspondent = c.Correspondent != null ? new CorrespondentDto { Id = c.Correspondent.Id, Name = c.Correspondent.Name } : null,
                    IsClosed = c.IsClosed
                })
                .ToListAsync();

            var tomorrow = today.AddDays(1);

            var overdueRemindersToday = await _context.Reminders
                .AsNoTracking()
                .Where(r =>
                    !r.IsCompleted &&
                    !r.IsDismissed &&
                    r.RemindTime >= today &&
                    r.RemindTime < now)
                .OrderBy(r => r.RemindTime)
                .Select(r => new GetReminderResponse
                {
                    Id = r.Id,
                    RemindTime = r.RemindTime,
                    Message = r.Message,
                    SendEmailMessage = r.SendEmailMessage,
                    IsEmailSent = r.IsEmailSent,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return new GetDashboardDataResponse
            {
                TotalCorrespondenceCount = totalCount,
                OpenCorrespondenceCount = openCount,
                Top10OpenCorrespondences = top10OpenCorrespondences,
                OverdueRemindersToday = overdueRemindersToday
            };
        }
    }
}