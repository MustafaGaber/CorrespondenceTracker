using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class Correspondence : Entity
    {
        public CorrespondenceDirection Direction { get; private set; }
        public PriorityLevel PriorityLevel { get; private set; }
        public string? IncomingNumber { get; private set; } = string.Empty;
        public DateOnly? IncomingDate { get; private set; }

        public Guid CorrespondentId { get; private set; }
        public virtual Correspondent? Correspondent { get; private set; }

        public string? OutgoingNumber { get; private set; }
        public DateOnly? OutgoingDate { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public virtual Department? Department { get; private set; }

        public string? Content { get; private set; }
        public string? Summary { get; private set; }

        public Guid? AssignedUserId { get; private set; }
        public virtual User? AssignedUser { get; private set; }

        public string? Notes { get; private set; }
        public bool IsClosed { get; private set; }
        public Guid? MainFileId { get; private set; }
        public virtual FileRecord? MainFile { get; private set; }

        private readonly List<FollowUp> _followUps = new();
        public virtual IReadOnlyList<FollowUp> FollowUps => _followUps.ToList();

        private readonly List<Attachment> _attachments = new();
        public virtual IReadOnlyList<Attachment> Attachments => _attachments.ToList();

        // Protected constructor for ORM
        protected Correspondence() { }

        // Public constructor
        public Correspondence(
            CorrespondenceDirection direction,
            PriorityLevel priorityLevel,
            string incomingNumber,
            DateOnly incomingDate,
            Guid correspondentId,
            string? outgoingNumber = null,
            DateOnly? outgoingDate = null,
            Guid? departmentId = null,
            string? content = null,
            string? summary = null,
            Guid? assignedUserId = null,
            string? notes = null,
            Guid? mainFileId = null,
            bool isClosed = false)
        {
            Direction = Guard.Against.EnumOutOfRange(direction, nameof(direction));
            IncomingNumber = Guard.Against.NullOrWhiteSpace(incomingNumber, nameof(incomingNumber));
            IncomingDate = Guard.Against.Null(incomingDate, nameof(incomingDate));
            CorrespondentId = Guard.Against.Default(correspondentId, nameof(correspondentId));
            PriorityLevel = Guard.Against.EnumOutOfRange(priorityLevel, nameof(priorityLevel));

            OutgoingNumber = outgoingNumber;
            OutgoingDate = outgoingDate;
            DepartmentId = departmentId;
            Content = content;
            Summary = summary;
            AssignedUserId = assignedUserId;
            Notes = notes;
            MainFileId = mainFileId;
            IsClosed = isClosed;

            if (outgoingDate.HasValue && outgoingDate.Value < incomingDate)
            {
                throw new ArgumentException("Outgoing date cannot be earlier than incoming date");
            }
        }

        public void CopyFrom(Correspondence other)
        {
            IncomingNumber = other.IncomingNumber;
            IncomingDate = other.IncomingDate;
            CorrespondentId = other.CorrespondentId;
            OutgoingNumber = other.OutgoingNumber;
            OutgoingDate = other.OutgoingDate;
            DepartmentId = other.DepartmentId;
            Summary = other.Summary;
            Notes = other.Notes;
            AssignedUserId = other.AssignedUserId;
            MainFileId = other.MainFileId;
        }
    }

    public enum CorrespondenceDirection
    {
        Incoming = 1,
        Outgoing = 2
    }

    public enum PriorityLevel
    {
        Critical = 10,
        UrgentAndImportant = 20,
        Urgent = 30,
        Important = 40,
        Medium = 50,
        Low = 60
    }
}