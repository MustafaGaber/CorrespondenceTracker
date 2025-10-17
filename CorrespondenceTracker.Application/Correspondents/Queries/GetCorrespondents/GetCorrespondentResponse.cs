namespace CorrespondenceTracker.Application.Correspondents.Queries.GetCorrespondents
{
    public class GetCorrespondentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

    }
}