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
                .Include(c => c.Correspondent)
                .Include(c => c.Subject)
                .Include(c => c.FollowUpUser)
                .Include(c => c.FollowUpUser)
                .Include(c => c.Department)
                .Include(c => c.File)
                .Include(c => c.Classifications)
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
                    Subject = c.Subject != null ? new SubjectDto { Id = c.Subject.Id, Name = c.Subject.Name } : null,
                    ResponsibleUser = c.ResponsibleUser != null ? new UserDto { Id = c.ResponsibleUser.Id, Name = c.ResponsibleUser.FullName } : null,
                    FollowUpUser = c.FollowUpUser != null ? new UserDto { Id = c.FollowUpUser.Id, Name = c.FollowUpUser.FullName } : null,
                    IsClosed = c.IsClosed,
                    OutgoingNumber = c.OutgoingNumber,
                    OutgoingDate = c.OutgoingDate,
                    FileId = c.FileId,
                    FileExtension = c.File == null ? null : c.File.Extension,
                    Department = c.Department == null ? null : new DepartmentDto { Id = c.Department.Id, Name = c.Department.Name },
                    CreatedAt = c.CreatedAt,
                    Classifications = c.Classifications.Select(c => new ClassificationDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
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