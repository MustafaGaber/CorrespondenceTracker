// DeleteReminderCommand.cs
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Reminders.Commands.DeleteReminder
{
    public interface IDeleteReminderCommand
    {
        Task Execute(Guid id);
    }

    public class DeleteReminderCommand : IDeleteReminderCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteReminderCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var reminder = await _context.Reminders.FindAsync(id)
                ?? throw new ArgumentException($"Reminder with ID {id} not found");

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}