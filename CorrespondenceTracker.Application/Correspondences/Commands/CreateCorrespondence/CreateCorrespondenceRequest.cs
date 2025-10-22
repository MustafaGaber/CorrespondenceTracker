using CorrespondenceTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;

public class CreateCorrespondenceRequest
{
    public IFormFile? File { get; init; }
    public string IncomingNumber { get; set; } = null!;
    public DateOnly? IncomingDate { get; set; }
    public Guid SenderId { get; set; }

    // Optional
    public string? OutgoingNumber { get; set; }
    public DateOnly? OutgoingDate { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Content { get; set; }
    public string? Summary { get; set; }
    public Guid? AssignedUserId { get; set; }
    public string? Notes { get; set; }
    public bool IsClosed { get; set; } = false;
    public CorrespondenceDirection? Direction { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    // New
    public Guid? SubjectId { get; set; }
    public List<Guid>? ClassificationIds { get; set; }
}
