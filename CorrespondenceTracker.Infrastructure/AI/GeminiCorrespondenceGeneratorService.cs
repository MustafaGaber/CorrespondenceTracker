using CorrespondenceTracker.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace CorrespondenceTracker.Infrastructure.AI
{
    public class GeminiCorrespondenceGeneratorService : IGeminiCorrespondenceGeneratorService
    {
        // NOTE: In a real application, this should be injected/configured securely.
        private readonly string _apiKey = "AIzaSyB-HofroNDF7YzhkTQd79FvMzv2i9xXPUg";
        private readonly string _modelName = "gemini-2.5-flash";

        public async Task<string> GenerateResponseAsync(string subjectHistory, string userPrompt)
        {
            string systemInstruction = "You are a professional administrative assistant. Your task is to draft a formal response letter for a subject based on its history and a user's instructions. The output should be only the text of the formal letter. Do not include a subject line, salutation, or closing unless explicitly requested in the prompt. Focus on clarity, professionalism, and directly addressing the history and prompt.";

            // Construct the final prompt
            string prompt = new StringBuilder()
                .AppendLine("Please draft a formal response letter (new outgoing correspondence) based on the following subject history and user instructions.")
                .AppendLine("--- SUBJECT HISTORY ---")
                .AppendLine(subjectHistory)
                .AppendLine("--- USER INSTRUCTIONS ---")
                .AppendLine(userPrompt)
                .AppendLine("--- GENERATE RESPONSE ---")
                .ToString();

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
                throw new HttpRequestException($"Correspondence generation request failed. Status: {response.StatusCode}, Response: {errorBody}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(jsonResponse);
            return document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "No content generated.";
        }
    }
}