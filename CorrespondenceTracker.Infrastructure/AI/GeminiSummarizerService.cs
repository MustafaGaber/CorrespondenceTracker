using CorrespondenceTracker.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace CorrespondenceTracker.Infrastructure.AI
{
    public class GeminiSummarizerService : IGeminiSummarizerService
    {
        private readonly string _apiKey = "AIzaSyB-HofroNDF7YzhkTQd79FvMzv2i9xXPUg";
        private readonly string _modelName = "gemini-2.5-flash";


        public GeminiSummarizerService()
        {
        }

        public async Task<string> SummarizeTextAsync(string originalText)
        {
            string prompt = $"Please summarize the following text concisely:\n---\n{originalText}";
            string systemInstruction = "You are a professional summarization assistant. Summarize the provided text concisely and accurately. The summary should be in the same language as the original text.";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:generateContent?key={_apiKey}";
            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                systemInstruction = new
                {
                    parts = new[] { new { text = systemInstruction } }
                }
            };

            return await ExecuteGeminiApiCallAsync(url, payload);
        }

        private async Task<string> ExecuteGeminiApiCallAsync(string url, object payload)
        {
            using var client = new HttpClient();
            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Summarization request failed. Status: {response.StatusCode}, Response: {errorBody}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(jsonResponse);
            return document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "No summary generated.";
        }
    }
}
