// CreateReminderRequest.cs
namespace CorrespondenceTracker.Application.Reminders.Commands.CreateReminder
{
    public class CreateReminderRequest
    {
        public DateTime RemindTime { get; set; }
        public string? Message { get; set; }
        public bool SendEmailMessage { get; set; }
    }
}