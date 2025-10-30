// CreateReminderCommand.cs
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Reminders.Commands.CreateReminder
{
    public interface ICreateReminderCommand
    {
        Task<Guid> Execute(Guid correspondenceId, CreateReminderRequest request);
    }

    public class CreateReminderCommand : ICreateReminderCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateReminderCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(Guid correspondenceId, CreateReminderRequest request)
        {
            var correspondence = await _context.Correspondences.FindAsync(correspondenceId)
                ?? throw new ArgumentException($"Correspondence with ID {correspondenceId} not found");

            var reminder = new Reminder(
                correspondenceId: correspondenceId,
                remindTime: request.RemindTime,
                sendEmailMessage: request.SendEmailMessage,
                message: request.Message
            );

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return reminder.Id;
        }
    }
}