using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class FollowUp : Entity
    {
        public Guid CorrespondenceId { get; private set; }
        public Guid? FileRecordId { get; private set; }
        public Guid? UserId { get; private set; }
        public DateOnly Date { get; private set; }
        public string Details { get; private set; } = string.Empty;

        public virtual Correspondence? Correspondence { get; private set; }
        public virtual User? User { get; private set; }
        public virtual FileRecord? FileRecord { get; private set; }

        protected FollowUp() { }

        public FollowUp(Guid correspondenceId, Guid? userId, DateOnly date, string details, Guid? fileRecordId = null)
        {
            CorrespondenceId = Guard.Against.Default(correspondenceId);
            Date = Guard.Against.Null(date);
            Details = Guard.Against.NullOrWhiteSpace(details);
            UserId = userId;
            FileRecordId = fileRecordId;
        }

        public void Update(Guid? userId, DateOnly date, string details, Guid? fileRecordId = null)
        {
            Date = Guard.Against.Null(date);
            Details = Guard.Against.NullOrWhiteSpace(details);
            UserId = userId;
            FileRecordId = fileRecordId;
        }
    }
}