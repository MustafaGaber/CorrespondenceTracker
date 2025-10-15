// FollowUp.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class FollowUp : Entity
    {
        public Guid CorrespondenceId { get; private set; }
        public DateOnly Date { get; private set; }
        public string Details { get; private set; } = string.Empty;
        public Guid? FileRecordId { get; private set; }

        public virtual Correspondence? Correspondence { get; private set; }
        public virtual FileRecord? FileRecord { get; private set; }

        // Protected constructor for ORM
        protected FollowUp() { }

        // Public constructor
        public FollowUp(Guid correspondenceId, DateOnly date, string details, Guid? fileRecordId = null)
        {
            CorrespondenceId = Guard.Against.Default(correspondenceId, nameof(correspondenceId));
            Date = Guard.Against.Null(date, nameof(date));
            Details = Guard.Against.NullOrWhiteSpace(details, nameof(details));
            FileRecordId = fileRecordId;
        }
    }
}