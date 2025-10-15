// Attachment.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Attachment : Entity
    {
        public Guid CorrespondenceId { get; private set; }
        public string? Name { get; private set; }
        public string? Note { get; private set; }
        public DateOnly? Date { get; private set; }
        public Guid FileRecordId { get; private set; }

        // Navigation
        public virtual Correspondence? Correspondence { get; private set; }
        public virtual FileRecord? FileRecord { get; private set; }

        // Protected constructor for ORM
        protected Attachment() { }

        // Public constructor
        public Attachment(Guid correspondenceId, Guid fileRecordId, string? name = null, string? note = null, DateOnly? date = null)
        {
            CorrespondenceId = Guard.Against.Default(correspondenceId, nameof(correspondenceId));
            FileRecordId = Guard.Against.Default(fileRecordId, nameof(fileRecordId));
            Name = name;
            Note = note;
            Date = date;
        }
    }
}