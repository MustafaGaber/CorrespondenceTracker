namespace CorrespondenceTracker.Application.Correspondents.Queries.GetCorrespondents
{
    public class GetCorrespondentsFilterModel
    {
        public string? Name { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}