// GetRemindersQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Reminders.Queries.GetReminders
{
    public interface IGetRemindersQuery
    {
        Task<List<GetReminderResponse>> Execute(Guid correspondenceId);
    }

    public class GetRemindersQuery : IGetRemindersQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetRemindersQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<GetReminderResponse>> Execute(Guid correspondenceId)
        {
            var reminders = await _context.Reminders
                .Where(r => r.CorrespondenceId == correspondenceId)
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

            return reminders;
        }
    }
}