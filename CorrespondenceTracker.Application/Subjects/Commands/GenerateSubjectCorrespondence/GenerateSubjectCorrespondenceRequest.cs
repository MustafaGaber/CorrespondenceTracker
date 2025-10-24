namespace CorrespondenceTracker.Application.Subjects.Commands.GenerateSubjectCorrespondence
{
    /// <summary>
    /// Represents the request to generate a new correspondence for a subject based on a text prompt.
    /// </summary>
    public class GenerateSubjectCorrespondenceRequest
    {


        /// <summary>
        /// The user's prompt or instructions for the response letter.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the correspondent who will receive this outgoing correspondence.
        /// </summary>
        public Guid RecipientId { get; set; }
    }
}