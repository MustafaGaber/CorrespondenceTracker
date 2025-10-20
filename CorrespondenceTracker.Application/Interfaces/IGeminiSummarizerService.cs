namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IGeminiSummarizerService
    {
        Task<string> SummarizeTextAsync(string originalText);

    }
}
