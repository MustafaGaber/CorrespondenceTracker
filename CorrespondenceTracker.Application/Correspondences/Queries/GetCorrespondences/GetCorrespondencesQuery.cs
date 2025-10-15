using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences
{
    public class GetCorrespondencesQuery : IGetCorrespondencesQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetCorrespondencesQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<GetCorrespondenceResponse>> Execute(GetCorrespondencesFilterModel filter)
        {
            var query = _context.Correspondences
                .Include(l => l.Correspondent)
                .Include(l => l.Department)
                .Include(l => l.AssignedUser)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(q =>
                    q.IncomingNumber.Contains(filter.SearchTerm) ||
                    q.OutgoingNumber.Contains(filter.SearchTerm) ||
                    q.Summary.Contains(filter.SearchTerm) ||
                    q.Content.Contains(filter.SearchTerm) ||
                    q.Correspondent.Name.Contains(filter.SearchTerm));
            }

            if (filter.Direction.HasValue)
            {
                query = query.Where(l => l.Direction == filter.Direction.Value);
            }

            if (filter.PriorityLevel.HasValue)
            {
                query = query.Where(l => l.PriorityLevel == filter.PriorityLevel.Value);
            }

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(l => l.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.AssignedUserId.HasValue)
            {
                query = query.Where(l => l.AssignedUserId == filter.AssignedUserId.Value);
            }

            if (filter.IsClosed.HasValue)
            {
                query = query.Where(l => l.IsClosed == filter.IsClosed.Value);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(l => l.IncomingDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(l => l.IncomingDate <= filter.ToDate.Value);
            }

            // Apply pagination
            var correspondences = await query
                .OrderByDescending(l => l.IncomingDate)
                .ThenByDescending(l => l.CreatedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return correspondences.Select(correspondence => new GetCorrespondenceResponse
            {
                Id = correspondence.Id,
                Direction = correspondence.Direction,
                PriorityLevel = correspondence.PriorityLevel,
                IncomingNumber = correspondence.IncomingNumber ?? string.Empty,
                IncomingDate = correspondence.IncomingDate ?? DateOnly.MinValue,
                OutgoingNumber = correspondence.OutgoingNumber,
                OutgoingDate = correspondence.OutgoingDate,
                CorrespondentName = correspondence.Correspondent?.Name ?? string.Empty,
                DepartmentName = correspondence.Department?.Name,
                Summary = correspondence.Summary,
                AssignedUserName = correspondence.AssignedUser?.FullName,
                IsClosed = correspondence.IsClosed,
                CreatedAt = correspondence.CreatedAt
            }).ToList();
        }
    }

    public interface IGetCorrespondencesQuery
    {
        Task<List<GetCorrespondenceResponse>> Execute(GetCorrespondencesFilterModel filter);
    }
}