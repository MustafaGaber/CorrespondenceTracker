using CorrespondenceTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;

public class UpdateCorrespondenceRequest
{
    public IFormFile? File { get; init; }
    public string? IncomingNumber { get; set; }
    public DateOnly? IncomingDate { get; set; }
    public Guid SenderId { get; set; }

    // Optional
    public string? OutgoingNumber { get; set; }
    public DateOnly? OutgoingDate { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Content { get; set; }
    public string? Summary { get; set; }
    public Guid? FollowUpUserId { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public string? Notes { get; set; }
    public bool IsClosed { get; set; } = false;
    public CorrespondenceDirection? Direction { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public Guid? SubjectId { get; set; }
    public List<Guid>? ClassificationIds { get; set; }
    public List<ReminderDto>? Reminders { get; set; }
}

public class ReminderDto
{
    public Guid? Id { get; set; }  // null = new reminder, has value = update existing
    public DateTime RemindTime { get; set; }
    public string? Message { get; set; }
    public bool SendEmailMessage { get; set; }
}