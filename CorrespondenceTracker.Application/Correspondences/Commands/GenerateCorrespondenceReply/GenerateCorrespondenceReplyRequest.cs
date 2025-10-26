namespace CorrespondenceTracker.Application.Subjects.Commands.GenerateCorrespondenceReply
{
    /// <summary>
    /// Represents the request to generate a reply to a specific correspondence.
    /// </summary>
    public class GenerateCorrespondenceReplyRequest
    {
        /// <summary>
        /// The user's prompt or instructions for the reply letter.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;
    }
}