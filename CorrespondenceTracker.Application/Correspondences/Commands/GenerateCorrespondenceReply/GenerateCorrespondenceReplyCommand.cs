using Ardalis.GuardClauses;
using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Application.Subjects.Queries.GetSubject;
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;
using System.Text;

namespace CorrespondenceTracker.Application.Subjects.Commands.GenerateCorrespondenceReply
{
    public interface IGenerateCorrespondenceReplyCommand
    {
        Task<GenerateCorrespondenceReplyResponse> Execute(Guid originalCorrespondenceId, GenerateCorrespondenceReplyRequest model);
    }
    public class GenerateCorrespondenceReplyCommand : IGenerateCorrespondenceReplyCommand
    {
        private readonly CorrespondenceDatabaseContext _context;
        private readonly IGetSubjectQuery _getSubjectQuery;
        private readonly IGeminiCorrespondenceGeneratorService _generatorService;
        private readonly IGeminiSummarizerService _summarizerService; // Kept for consistency

        public GenerateCorrespondenceReplyCommand(
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

        public async Task<GenerateCorrespondenceReplyResponse> Execute(Guid originalCorrespondenceId, GenerateCorrespondenceReplyRequest model)
        {
            Guard.Against.Default(originalCorrespondenceId);
            Guard.Against.NullOrWhiteSpace(model.Prompt, nameof(model.Prompt));

            // 1. Fetch the original correspondence
            var originalCorrespondence = await _context.Correspondences.FindAsync(originalCorrespondenceId);
            Guard.Against.NotFound(originalCorrespondenceId, originalCorrespondence, $"Original correspondence with ID {originalCorrespondenceId} not found.");

            // 2. Validate the original correspondence
            if (originalCorrespondence.Direction != CorrespondenceDirection.Incoming)
            {
                throw new InvalidOperationException("Cannot generate a reply for an outgoing correspondence.");
            }
            Guard.Against.Default(originalCorrespondence.SubjectId, nameof(originalCorrespondence.SubjectId), "The original correspondence is not linked to a subject.");
            Guard.Against.Default(originalCorrespondence.CorrespondentId, nameof(originalCorrespondence.CorrespondentId), "The original correspondence does not have a valid correspondent.");

            // 3. Extract key IDs for the new reply
            var subjectId = originalCorrespondence.SubjectId.Value;
            var recipientId = originalCorrespondence.CorrespondentId; // The sender of the original is the recipient of the reply

            // 4. Fetch Subject History using the existing query
            var subjectResponse = await _getSubjectQuery.Execute(subjectId);
            Guard.Against.NotFound(subjectId, subjectResponse, $"Subject with ID {subjectId} not found.");

            // Check if recipient (the original sender) exists
            var recipient = await _context.Correspondents.FindAsync(recipientId)
                ?? throw new ArgumentException($"Correspondent (recipient) with ID {recipientId} not found");

            // 5. Format the subject history for the Gemini API
            string subjectHistory = FormatSubjectHistory(subjectResponse);

            // 6. Call Gemini to generate the correspondence content
            string generatedContent = await _generatorService.GenerateResponseAsync(
                subjectHistory,
                model.Prompt);

            // 7. Summarize and Save (Kept commented out, matching your existing command)
            /* string generatedSummary = await _summarizerService.SummarizeTextAsync(generatedContent);

             var newCorrespondence = new Correspondence(
                 direction: CorrespondenceDirection.Outgoing,
                 priorityLevel: PriorityLevel.Medium, // Defaulting to Medium
                 subjectId: subjectId,
                 correspondentId: recipientId, // This is the ID of the original sender
                 content: generatedContent,
                 summary: generatedSummary
             );

             _context.Correspondences.Add(newCorrespondence);
             await _context.SaveChangesAsync();*/

            // 8. Return the response
            return new GenerateCorrespondenceReplyResponse
            {
                //Id = newCorrespondence.Id,
                GeneratedContent = generatedContent,
                //GeneratedSummary = generatedSummary
            };
        }

        // This private method is duplicated from GenerateSubjectCorrespondenceCommand
        // In a real app, this might be moved to a shared service or helper class.
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