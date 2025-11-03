// CorrespondenceReportModel.cs
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Reports.Queries.GetCorrespondencesReportData
{
    public class CorrespondenceReportModel
    {
        public required Guid Id { get; init; }
        public required CorrespondenceDirection Direction { get; init; }
        public required PriorityLevel PriorityLevel { get; init; }
        public required string? IncomingNumber { get; init; }
        public required DateOnly? IncomingDate { get; init; }
        public required string? OutgoingNumber { get; init; }
        public required DateOnly? OutgoingDate { get; init; }
        public required string? CorrespondentName { get; init; }
        public required string? DepartmentName { get; init; }
        public required string? Subject { get; init; }
        public required string? Summary { get; init; }
        public required string? ResponsibleUserName { get; init; }
        public required string? FollowUpUserName { get; init; }
        public required bool IsClosed { get; init; }
        public required string Classifications { get; init; } // Comma-separated list
    }
}