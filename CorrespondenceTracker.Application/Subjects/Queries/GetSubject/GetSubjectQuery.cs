// File: CorrespondenceTracker.Application.Subjects.Queries.GetSubject/GetSubjectQuery.cs

using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Subjects.Queries.GetSubject
{
    public class GetSubjectQuery : IGetSubjectQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetSubjectQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<GetSubjectResponse?> Execute(Guid id)
        {
            // Fetch the Subject and include its Correspondences.
            // For each Correspondence, include its FollowUps.
            var subject = await _context.Subjects
                .Where(s => s.Id == id)
                .Select(s => new GetSubjectResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Correspondences = _context.Correspondences
                        .Where(c => c.SubjectId == s.Id)
                        .Include(c => c.FollowUps) // Include FollowUps for each Correspondence
                        .Select(c => new SubjectCorrespondenceDto
                        {
                            Id = c.Id,
                            Direction = c.Direction,
                            PriorityLevel = c.PriorityLevel,
                            IncomingNumber = c.IncomingNumber ?? string.Empty,
                            IncomingDate = c.IncomingDate,
                            OutgoingNumber = c.OutgoingNumber,
                            OutgoingDate = c.OutgoingDate,
                            Summary = c.Summary,
                            Content = c.Content,
                            IsClosed = c.IsClosed,
                            CreatedAt = c.CreatedAt,
                            // Map Follow-Ups to DTOs
                            FollowUps = c.FollowUps.Select(f => new SubjectFollowUpDto
                            {
                                Id = f.Id,
                                Date = f.Date,
                                Details = f.Details
                            }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return subject;
        }
    }

    public interface IGetSubjectQuery
    {
        Task<GetSubjectResponse?> Execute(Guid id);
    }
}