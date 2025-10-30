// UpdateReminderCommand.cs
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Reminders.Commands.UpdateReminder
{
    public interface IUpdateReminderCommand
    {
        Task Execute(Guid id, UpdateReminderRequest request);
    }

    public class UpdateReminderCommand : IUpdateReminderCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateReminderCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, UpdateReminderRequest request)
        {
            var reminder = await _context.Reminders.FindAsync(id)
                ?? throw new ArgumentException($"Reminder with ID {id} not found");

            reminder.Update(
                request.RemindTime,
                request.SendEmailMessage,
                request.Message
            );

            await _context.SaveChangesAsync();
        }
    }
}