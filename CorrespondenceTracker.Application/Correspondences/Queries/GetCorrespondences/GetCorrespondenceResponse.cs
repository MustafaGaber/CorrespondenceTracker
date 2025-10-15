using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences
{
    public class GetCorrespondenceResponse
    {
        public Guid Id { get; set; }
        public CorrespondenceDirection Direction { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string IncomingNumber { get; set; } = string.Empty;
        public DateOnly IncomingDate { get; set; }
        public string? OutgoingNumber { get; set; }
        public DateOnly? OutgoingDate { get; set; }
        public string CorrespondentName { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? Summary { get; set; }
        public string? AssignedUserName { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}