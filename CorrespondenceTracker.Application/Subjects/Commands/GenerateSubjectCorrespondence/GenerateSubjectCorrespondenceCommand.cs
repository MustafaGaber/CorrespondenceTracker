using Ardalis.GuardClauses;
using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Application.Subjects.Queries.GetSubject;
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;
using System.Text;

namespace CorrespondenceTracker.Application.Subjects.Commands.GenerateSubjectCorrespondence
{
    public interface IGenerateSubjectCorrespondenceCommand
    {
        Task<GenerateSubjectCorrespondenceResponse> Execute(Guid subjectId, GenerateSubjectCorrespondenceRequest model);
    }

    public class GenerateSubjectCorrespondenceCommand : IGenerateSubjectCorrespondenceCommand
    {
        private readonly CorrespondenceDatabaseContext _context;
        private readonly IGetSubjectQuery _getSubjectQuery;
        private readonly IGeminiCorrespondenceGeneratorService _generatorService;
        private readonly IGeminiSummarizerService _summarizerService;

        public GenerateSubjectCorrespondenceCommand(
            CorrespondenceDatabaseContext context,
            IGetSubjectQuery getSubjectQuery,
            IGeminiCorrespondenceGeneratorService generatorService,
            IGeminiSummarizerService summarizerService)
        {
            _context = context;
            _getSubjectQuery = getSubjectQuery;
            _generatorService = generatorService;
            _summarizerService = summarizerService;
        }

        public async Task<GenerateSubjectCorrespondenceResponse> Execute(Guid subjectId, GenerateSubjectCorrespondenceRequest model)
        {
            Guard.Against.Default(subjectId);
            Guard.Against.NullOrWhiteSpace(model.Prompt, nameof(model.Prompt));
            Guard.Against.Default(model.RecipientId, nameof(model.RecipientId));

            // 1. Fetch Subject History using the existing query
            var subjectResponse = await _getSubjectQuery.Execute(subjectId);
            Guard.Against.NotFound(subjectId, subjectResponse, $"Subject with ID {subjectId} not found.");

            // Check if recipient exists
            var correspondent = await _context.Correspondents.FindAsync(model.RecipientId)
                ?? throw new ArgumentException($"Correspondent with ID {model.RecipientId} not found");

            // 2. Format the subject history for the Gemini API
            string subjectHistory = FormatSubjectHistory(subjectResponse);

            // 3. Call Gemini to generate the correspondence content
            string generatedContent = await _generatorService.GenerateResponseAsync(
                subjectHistory,
                model.Prompt);

            // 4. Summarize the generated content
            string generatedSummary = await _summarizerService.SummarizeTextAsync(generatedContent);

            // 5. Create and save the new outgoing correspondence entity
            var newCorrespondence = new Correspondence(
                direction: CorrespondenceDirection.Outgoing,
                priorityLevel: PriorityLevel.Medium, // Defaulting to Medium
                subjectId: subjectId,
                correspondentId: model.RecipientId,
                content: generatedContent,
                summary: generatedSummary
            );

            _context.Correspondences.Add(newCorrespondence);
            await _context.SaveChangesAsync();

            // 6. Return the response
            return new GenerateSubjectCorrespondenceResponse
            {
                Id = newCorrespondence.Id,
                GeneratedContent = generatedContent,
                GeneratedSummary = generatedSummary
            };
        }

        private string FormatSubjectHistory(GetSubjectResponse subjectResponse)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Subject: {subjectResponse.Name}");
            sb.AppendLine("---");

            foreach (var c in subjectResponse.Correspondences.OrderBy(c => c.CreatedAt))
            {
                // Use Content if available, otherwise use Summary
                string correspondenceText = string.IsNullOrWhiteSpace(c.Content) ? c.Summary : c.Content;

                sb.AppendLine($"[CORRESPONDENCE {c.Id} - {c.Direction} - {c.CreatedAt:yyyy-MM-dd}]");
                sb.AppendLine(correspondenceText ?? "No content available.");

                if (c.FollowUps.Any())
                {
                    sb.AppendLine("  --- Follow-Ups:");
                    foreach (var f in c.FollowUps.OrderBy(f => f.Date))
                    {
                        sb.AppendLine($"  - [{f.Date:yyyy-MM-dd}] {f.Details}");
                    }
                }
                sb.AppendLine("---");
            }

            return sb.ToString();
        }
    }
}