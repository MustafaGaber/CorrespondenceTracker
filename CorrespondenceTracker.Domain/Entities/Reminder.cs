namespace CorrespondenceTracker.Domain.Entities
{
    using Ardalis.GuardClauses;

    public class Reminder : Entity
    {
        public Guid CorrespondenceId { get; private set; }
        public Correspondence Correspondence { get; private set; }
        public DateTime RemindTime { get; private set; }
        public string? Message { get; private set; }
        public bool SendEmailMessage { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDismissed { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public bool IsEmailSent { get; private set; }
        public DateTime? EmailSentAt { get; private set; }

        private readonly List<User> _usersToRemind = new();
        public virtual IReadOnlyList<User> UsersToRemind => _usersToRemind.ToList();

        protected Reminder() { }

        // Public constructor
        public Reminder(
            Guid correspondenceId,
            DateTime remindTime,
            bool sendEmailMessage,
            string? message = null)
        {
            CorrespondenceId = Guard.Against.Default(correspondenceId, nameof(correspondenceId));
            RemindTime = Guard.Against.Default(remindTime, nameof(remindTime));
            Guard.Against.OutOfRange(remindTime, nameof(remindTime), DateTime.Now.AddMinutes(-1), DateTime.MaxValue);
            SendEmailMessage = sendEmailMessage;
            Message = message;
            IsCompleted = false;
            IsDismissed = false;
            IsEmailSent = false;
            CreatedAt = DateTime.Now;
        }

        // Update method for modifying reminder properties
        public void Update(DateTime remindTime, bool sendEmailMessage, string? message = null)
        {
            Guard.Against.Default(remindTime);

            if (RemindTime != remindTime)
            {
                Guard.Against.OutOfRange(remindTime, nameof(remindTime), DateTime.Now.AddMinutes(-1), DateTime.MaxValue);
            }

            RemindTime = remindTime;
            SendEmailMessage = sendEmailMessage;
            Message = message;
            UpdatedAt = DateTime.Now;
        }

        // Business methods
        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                CompletedAt = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
        }

        public void MarkAsDismissed()
        {
            if (!IsDismissed)
            {
                IsDismissed = true;
                UpdatedAt = DateTime.Now;
            }
        }

        public void Reschedule(DateTime newRemindTime)
        {
            Guard.Against.Default(newRemindTime, nameof(newRemindTime));
            Guard.Against.OutOfRange(newRemindTime, nameof(newRemindTime), DateTime.Now.AddMinutes(-1), DateTime.MaxValue);

            RemindTime = newRemindTime;
            IsDismissed = false; // Reset dismissal when rescheduling
            IsEmailSent = false; // Reset email sent status when rescheduling
            UpdatedAt = DateTime.Now;
        }

        public void UpdateMessage(string? message)
        {
            Message = message;
            UpdatedAt = DateTime.Now;
        }

        public void MarkEmailAsSent()
        {
            if (!IsEmailSent)
            {
                IsEmailSent = true;
                EmailSentAt = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
        }

        // Helper property to check if reminder is active
        public bool IsActive => !IsCompleted && !IsDismissed && RemindTime > DateTime.Now;

        // Helper property to check if reminder is overdue
        public bool IsOverdue => !IsCompleted && !IsDismissed && RemindTime <= DateTime.Now;
    }
}