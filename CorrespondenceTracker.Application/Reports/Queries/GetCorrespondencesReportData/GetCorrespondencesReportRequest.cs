using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Reports.Queries.GetCorrespondencesReportData
{
    public class GetCorrespondencesReportRequest
    {
        public string ReportTitle { get; set; } = "بيان بالمكاتبات";

        // Filtering Properties
        public DateOnly? IncomingDateFrom { get; set; }
        public DateOnly? IncomingDateTo { get; set; }
        public DateOnly? OutgoingDateFrom { get; set; }
        public DateOnly? OutgoingDateTo { get; set; }
        public List<Guid> CorrespondentIds { get; set; } = new List<Guid>();
        public List<Guid> DepartmentIds { get; set; } = new List<Guid>();
        public List<Guid> ResponsibleUserIds { get; set; } = new List<Guid>();
        public List<Guid> FollowUpUserIds { get; set; } = new List<Guid>();
        public List<Guid> ClassificationIds { get; set; } = new List<Guid>();
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
        public bool? IsClosed { get; set; } // null = All, true = Closed, false = Open
        public List<PriorityLevel> PriorityLevels { get; set; } = new List<PriorityLevel>();
        public List<CorrespondenceDirection> Directions { get; set; } = new List<CorrespondenceDirection>();

        // Column Control Properties
        public List<string>? VisibleColumns { get; set; }
        public Dictionary<string, string> ColumnNames { get; set; } = new Dictionary<string, string>();
        public List<string> ColumnOrder { get; set; } = new List<string>();
    }
}
