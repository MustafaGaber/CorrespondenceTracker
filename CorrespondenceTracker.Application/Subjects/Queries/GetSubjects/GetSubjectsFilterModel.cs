namespace CorrespondenceTracker.Application.Subjects.Queries.GetSubjects
{
    public class GetSubjectsFilterModel
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}