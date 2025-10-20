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
                .Include(l => l.Classifications)
                .Include(l => l.Subject)
                .Include(l => l.File)
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
                FileId = correspondence.FileId,
                FileExtension = correspondence.File?.Extension,
                Direction = correspondence.Direction,
                PriorityLevel = correspondence.PriorityLevel,
                IncomingNumber = correspondence.IncomingNumber ?? string.Empty,
                IncomingDate = correspondence.IncomingDate ?? DateOnly.MinValue,
                OutgoingNumber = correspondence.OutgoingNumber,
                OutgoingDate = correspondence.OutgoingDate,
                Correspondent = correspondence.Correspondent != null ? new CorrespondentDto
                {
                    Id = correspondence.Correspondent.Id,
                    Name = correspondence.Correspondent.Name
                } : null,
                Department = correspondence.Department != null ? new DepartmentDto
                {
                    Id = correspondence.Department.Id,
                    Name = correspondence.Department.Name
                } : null,
                Summary = correspondence.Summary,
                AssignedUser = correspondence.AssignedUser != null ? new UserDto
                {
                    Id = correspondence.AssignedUser.Id,
                    Name = correspondence.AssignedUser.FullName
                } : null,
                Subject = correspondence.Subject != null ? new SubjectDto
                {
                    Id = correspondence.Subject.Id,
                    Name = correspondence.Subject.Name
                } : null,
                IsClosed = correspondence.IsClosed,
                CreatedAt = correspondence.CreatedAt,
                Classifications = correspondence.Classifications.Select(c => new ClassificationDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            }).ToList();
        }
    }

    public interface IGetCorrespondencesQuery
    {
        Task<List<GetCorrespondenceResponse>> Execute(GetCorrespondencesFilterModel filter);
    }
}