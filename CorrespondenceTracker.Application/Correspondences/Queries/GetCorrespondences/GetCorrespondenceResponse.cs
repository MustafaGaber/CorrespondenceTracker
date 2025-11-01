using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences
{
    public class GetCorrespondenceItemResponse
    {
        public required Guid Id { get; set; }
        public required Guid? FileId { get; set; }
        public required string? FileExtension { get; set; }
        public required CorrespondenceDirection Direction { get; set; }
        public required PriorityLevel PriorityLevel { get; set; }
        public required string IncomingNumber { get; set; } = string.Empty;
        public required DateOnly IncomingDate { get; set; }
        public required string? OutgoingNumber { get; set; }
        public required DateOnly? OutgoingDate { get; set; }
        public required CorrespondentDto? Correspondent { get; set; }
        public required SubjectDto? Subject { get; set; }
        public required DepartmentDto? Department { get; set; }
        public string? Summary { get; set; }
        public required UserDto? ResponsibleUser { get; set; }
        public required UserDto? FollowUpUser { get; set; }
        public required bool IsClosed { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required List<ClassificationDto> Classifications { get; set; } = new();
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
}