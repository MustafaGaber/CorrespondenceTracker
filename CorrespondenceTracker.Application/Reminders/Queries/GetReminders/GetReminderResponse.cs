// GetReminderResponse.cs (for list)
namespace CorrespondenceTracker.Application.Reminders.Queries.GetReminders
{
    public class GetReminderResponse
    {
        public Guid Id { get; set; }
        public DateTime RemindTime { get; set; }
        public string? Message { get; set; }
        public bool SendEmailMessage { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}