namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IGeminiCorrespondenceGeneratorService
    {
        /// <summary>
        /// Generates the content of a new correspondence based on the subject history and a user prompt.
        /// </summary>
        /// <param name="subjectHistory">A formatted string containing all previous correspondences and follow-ups.</param>
        /// <param name="userPrompt">The user's specific instructions for the response.</param>
        /// <returns>The generated correspondence text.</returns>
        Task<string> GenerateResponseAsync(string subjectHistory, string userPrompt);
    }
}