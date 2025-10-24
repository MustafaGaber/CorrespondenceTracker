namespace CorrespondenceTracker.Application.Subjects.Commands.GenerateSubjectCorrespondence
{
    /// <summary>
    /// Represents the response after successfully generating a new correspondence.
    /// </summary>
    public class GenerateSubjectCorrespondenceResponse
    {
        public Guid Id { get; set; }
        public string GeneratedContent { get; set; } = string.Empty;
        public string GeneratedSummary { get; set; } = string.Empty;
    }
}