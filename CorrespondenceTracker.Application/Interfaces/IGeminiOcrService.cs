namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IGeminiOcrService
    {
        Task<string> ExtractTextFromFileAsync(string filePath);

    }
}
