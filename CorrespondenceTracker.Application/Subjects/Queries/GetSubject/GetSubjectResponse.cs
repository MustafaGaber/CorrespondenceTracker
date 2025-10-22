// File: CorrespondenceTracker.Application.Subjects.Queries.GetSubject/GetSubjectResponse.cs

using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Subjects.Queries.GetSubject
{
    public class GetSubjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubjectCorrespondenceDto> Correspondences { get; set; } = new();
    }

    public class SubjectCorrespondenceDto
    {
        public Guid Id { get; set; }
        public CorrespondenceDirection Direction { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string IncomingNumber { get; set; } = string.Empty;
        public DateOnly? IncomingDate { get; set; }
        public string? OutgoingNumber { get; set; }
        public DateOnly? OutgoingDate { get; set; }
        public string? Content { get; set; }
        public string? Summary { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SubjectFollowUpDto> FollowUps { get; set; } = new();
    }

    public class SubjectFollowUpDto
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}