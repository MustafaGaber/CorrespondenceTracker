// UpdateReminderRequest.cs
namespace CorrespondenceTracker.Application.Reminders.Commands.UpdateReminder
{
    public class UpdateReminderRequest
    {
        public DateTime RemindTime { get; set; }
        public string? Message { get; set; }
        public bool SendEmailMessage { get; set; }
    }
}