using CorrespondenceTracker.Domain.Entities;

public class CreateCorrespondenceRequest
{
    public string IncomingNumber { get; set; } = null!;
    public DateOnly IncomingDate { get; set; }
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

    // New
    public Guid? SubjectId { get; set; }
    public List<Guid>? ClassificationIds { get; set; }
}
