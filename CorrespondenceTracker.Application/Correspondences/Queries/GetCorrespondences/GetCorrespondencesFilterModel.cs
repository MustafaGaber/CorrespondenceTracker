using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences
{
    public class GetCorrespondencesFilterModel
    {
        public string? SearchTerm { get; set; }
        public CorrespondenceDirection? Direction { get; set; }
        public PriorityLevel? PriorityLevel { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public bool? IsClosed { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}