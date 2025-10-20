using CorrespondenceTracker.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace GeminiOcr
{
    public class GeminiOcrService : IGeminiOcrService
    {
        private readonly string _apiKey = "AIzaSyB-HofroNDF7YzhkTQd79FvMzv2i9xXPUg";
        private readonly string _modelName = "gemini-2.5-flash";
        private readonly string _pdfPrompt = "اقرأ كل النصوص الموجودة في هذا الملف بصيغة PDF بدقة، وانسخها كاملة بدون أي اختصار.";
        private readonly string _imagePrompt = "اقرأ كل النصوص الظاهرة في هذه الصورة بدقة، وانسخها بالكامل بدون أي اختصار.";
        public GeminiOcrService()
        {
        }

        public async Task<string> ExtractTextFromFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new ArgumentException("Invalid file path");

            string mimeType = GetMimeType(filePath);
            string base64File = ConvertFileToBase64(filePath);

            // 🟢 Arabic prompt for OCR
            string prompt = mimeType == "application/pdf" ? _pdfPrompt : _imagePrompt;

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:generateContent?key={_apiKey}";
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt },
                            new
                            {
                                inlineData = new
                                {
                                    mimeType = mimeType,
                                    data = base64File
                                }
                            }
                        }
                    }
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
                throw new HttpRequestException($"OCR request failure: {response.StatusCode}. الاستجابة: {errorBody}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(jsonResponse);
            return document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "لم يتم استخراج أي نص."
                .Replace(_pdfPrompt, "")
                .Replace(_imagePrompt, "");
        }

        private string ConvertFileToBase64(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(fileBytes);
        }

        private string GetMimeType(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                _ => throw new NotSupportedException($"Unsupported file type: {extension}")
            };
        }
    }
}
