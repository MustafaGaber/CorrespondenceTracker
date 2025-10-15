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

        public Guid? SubjectId { get; private set; }
        public virtual Subject? Subject { get; private set; }

        public string? Notes { get; private set; }
        public bool IsClosed { get; private set; }
        public Guid? MainFileId { get; private set; }
        public virtual FileRecord? MainFile { get; private set; }

        private readonly List<FollowUp> _followUps = new();
        public virtual IReadOnlyList<FollowUp> FollowUps => _followUps.ToList();

        private readonly List<Classification> _classifications = new();
        public virtual IReadOnlyList<Classification> Classifications => _classifications.ToList();

        private readonly List<Attachment> _attachments = new();
        public virtual IReadOnlyList<Attachment> Attachments => _attachments.ToList();

        private readonly List<Reminder> _reminders = new();
        public virtual IReadOnlyList<Reminder> Reminders => _reminders.ToList();

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
            bool isClosed = false,
            Guid? subjectId = null,
            IEnumerable<Classification>? classifications = null)
        {
            ApplyValidations(
                direction,
                priorityLevel,
                incomingNumber,
                incomingDate,
                outgoingNumber,
                outgoingDate,
                correspondentId
            );

            Direction = direction;
            PriorityLevel = priorityLevel;
            IncomingNumber = incomingNumber;
            IncomingDate = incomingDate;
            CorrespondentId = correspondentId;
            OutgoingNumber = outgoingNumber;
            OutgoingDate = outgoingDate;
            DepartmentId = departmentId;
            Content = content;
            Summary = summary;
            AssignedUserId = assignedUserId;
            Notes = notes;
            MainFileId = mainFileId;
            IsClosed = isClosed;
            SubjectId = subjectId;

            if (classifications != null)
            {
                foreach (var c in classifications)
                {
                    _classifications.Add(c);
                }
            }
        }

        // ✅ New Update method with full validations
        public void Update(
            string incomingNumber,
            DateOnly incomingDate,
            string? outgoingNumber,
            DateOnly? outgoingDate,
            Guid? departmentId,
            string? content,
            string? summary,
            Guid? assignedUserId,
            string? notes,
            bool isClosed,
            Guid? subjectId,
            IEnumerable<Classification>? classifications = null,
            CorrespondenceDirection? direction = null,
            PriorityLevel? priorityLevel = null)
        {
            var newDirection = direction ?? Direction;
            var newPriority = priorityLevel ?? PriorityLevel;

            ApplyValidations(
                newDirection,
                newPriority,
                incomingNumber,
                incomingDate,
                outgoingNumber,
                outgoingDate,
                CorrespondentId // CorrespondentId is immutable after creation
            );

            Direction = newDirection;
            PriorityLevel = newPriority;
            IncomingNumber = incomingNumber;
            IncomingDate = incomingDate;
            OutgoingNumber = outgoingNumber;
            OutgoingDate = outgoingDate;
            DepartmentId = departmentId;
            Content = content;
            Summary = summary;
            AssignedUserId = assignedUserId;
            Notes = notes;
            IsClosed = isClosed;
            SubjectId = subjectId;

            // Reset and replace classifications
            _classifications.Clear();
            if (classifications != null)
            {
                foreach (var c in classifications)
                {
                    _classifications.Add(c);
                }
            }
        }

        // ✅ Private shared validation logic
        private static void ApplyValidations(
            CorrespondenceDirection direction,
            PriorityLevel priorityLevel,
            string incomingNumber,
            DateOnly incomingDate,
            string? outgoingNumber,
            DateOnly? outgoingDate,
            Guid correspondentId)
        {
            Guard.Against.EnumOutOfRange(direction, nameof(direction));
            Guard.Against.EnumOutOfRange(priorityLevel, nameof(priorityLevel));
            Guard.Against.NullOrWhiteSpace(incomingNumber, nameof(incomingNumber));
            Guard.Against.Null(incomingDate, nameof(incomingDate));
            Guard.Against.Default(correspondentId, nameof(correspondentId));

            if (outgoingDate.HasValue && outgoingDate.Value < incomingDate)
            {
                throw new ArgumentException("Outgoing date cannot be earlier than incoming date");
            }
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
