namespace CorrespondenceTracker.Application.Subjects.Queries.GetSubjects
{
    public class GetSubjectResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}