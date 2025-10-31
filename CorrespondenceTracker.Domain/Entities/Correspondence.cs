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

        public Guid? FollowUpUserId { get; private set; }
        public virtual User? FollowUpUser { get; private set; }

        public Guid? ResponsibleUserId { get; private set; }
        public virtual User? ResponsibleUser { get; private set; }

        public Guid? SubjectId { get; private set; }
        public virtual Subject? Subject { get; private set; }

        public string? Notes { get; private set; }
        public bool IsClosed { get; private set; }
        public string? FinalAction { get; private set; }

        public Guid? FileId { get; private set; }
        public virtual FileRecord? File { get; private set; }

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
            Guid correspondentId,
            string? incomingNumber = null,
            DateOnly? incomingDate = null,
            string? outgoingNumber = null,
            DateOnly? outgoingDate = null,
            Guid? departmentId = null,
            string? content = null,
            string? summary = null,
            Guid? followUpUserId = null,
            Guid? responsibleUserId = null,
            string? notes = null,
            Guid? fileId = null,
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
            FollowUpUserId = followUpUserId;
            ResponsibleUserId = responsibleUserId;
            Notes = notes;
            FileId = fileId;
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

        public void Update(
            string incomingNumber,
            DateOnly? incomingDate,
            string? outgoingNumber,
            DateOnly? outgoingDate,
            Guid? departmentId,
            string? content,
            string? summary,
            Guid? followUpUserId,
            Guid? responsibleUserId,
            string? notes,
            Guid? fileId,
            bool isClosed,
            Guid? subjectId,
            IEnumerable<Classification>? classifications,
            CorrespondenceDirection? direction,
            PriorityLevel? priorityLevel)
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
                CorrespondentId
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
            FollowUpUserId = followUpUserId;
            ResponsibleUserId = responsibleUserId;
            Notes = notes;
            IsClosed = isClosed;
            SubjectId = subjectId;
            if (fileId.HasValue) FileId = fileId;

            _classifications.Clear();
            if (classifications != null)
            {
                foreach (var c in classifications)
                {
                    _classifications.Add(c);
                }
            }
        }

        private static void ApplyValidations(
            CorrespondenceDirection direction,
            PriorityLevel priorityLevel,
            string? incomingNumber,
            DateOnly? incomingDate,
            string? outgoingNumber,
            DateOnly? outgoingDate,
            Guid correspondentId)
        {
            Guard.Against.EnumOutOfRange(direction);
            Guard.Against.EnumOutOfRange(priorityLevel);
            Guard.Against.Default(correspondentId);

            if (incomingDate.HasValue && outgoingDate.HasValue && outgoingDate.Value > incomingDate.Value)
            {
                throw new ArgumentException("Incoming date cannot be earlier than outgoing date");
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