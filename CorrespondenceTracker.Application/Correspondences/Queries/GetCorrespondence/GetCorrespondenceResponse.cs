using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondence
{
    public class GetCorrespondenceResponse
    {
        public Guid Id { get; set; }
        public Guid? FileId { get; set; }
        public string? FileExtension { get; set; }
        public CorrespondenceDirection Direction { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string IncomingNumber { get; set; } = string.Empty;
        public DateOnly IncomingDate { get; set; }
        public string? OutgoingNumber { get; set; }
        public DateOnly? OutgoingDate { get; set; }
        public CorrespondentDto? Correspondent { get; set; }
        public SubjectDto? Subject { get; set; }
        public DepartmentDto? Department { get; set; }
        public string? Content { get; set; }
        public string? Summary { get; set; }
        public UserDto? AssignedUser { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ClassificationDto> Classifications { get; set; } = new();
        // New property for Follow-Ups
        public List<FollowUpDto> FollowUps { get; set; } = new();
    }

    public class CorrespondentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ClassificationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // New DTO for Follow-Ups
    public class FollowUpDto
    {
        public Guid Id { get; set; }
        public Guid? FileRecordId { get; set; }
        public UserDto? User { get; set; }
        public DateOnly Date { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}