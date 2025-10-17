namespace CorrespondenceTracker.Application.Classifications.Queries.GetClassifications
{
    public class GetClassificationsFilterModel
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}