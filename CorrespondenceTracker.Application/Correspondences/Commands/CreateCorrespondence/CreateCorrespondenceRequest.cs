using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondence
{
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

        // Collections
        public List<Guid>? ClassificationIds { get; set; }
    }
}
