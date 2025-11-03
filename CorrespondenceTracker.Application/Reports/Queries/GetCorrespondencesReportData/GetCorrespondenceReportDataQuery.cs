// GetCorrespondenceReportDataQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CorrespondenceTracker.Application.Reports.Queries.GetCorrespondencesReportData
{
    public class GetCorrespondenceReportDataQuery : IGetCorrespondenceReportDataQuery
    {
        private readonly CorrespondenceDatabaseContext _dbContext;

        public GetCorrespondenceReportDataQuery(CorrespondenceDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<CorrespondenceReportModel>> Execute(GetCorrespondencesReportRequest request, ClaimsPrincipal user)
        {
            var query = _dbContext.Correspondences
                .Include(c => c.Correspondent)
                .Include(c => c.Department)
                .Include(c => c.Subject)
                .Include(c => c.ResponsibleUser)
                .Include(c => c.FollowUpUser)
                .Include(c => c.Classifications)
                .OrderByDescending(c => c.IncomingDate)
                .AsQueryable();

            // Apply security trimming
            // I'm assuming Correspondence has an AuthorityId like your Project entity.
            // Adjust this if your authorization logic is different.
            // query = query.Where(c => c.AuthorityId == user.AuthorityId());

            // Apply filters from the request
            if (request.Directions != null && request.Directions.Any())
            {
                query = query.Where(c => request.Directions.Contains(c.Direction));
            }

            if (request.PriorityLevels != null && request.PriorityLevels.Any())
            {
                query = query.Where(c => request.PriorityLevels.Contains(c.PriorityLevel));
            }

            if (request.CorrespondentIds != null && request.CorrespondentIds.Any())
            {
                query = query.Where(c => request.CorrespondentIds.Contains(c.CorrespondentId));
            }

            if (request.DepartmentIds != null && request.DepartmentIds.Any())
            {
                query = query.Where(c => c.DepartmentId.HasValue && request.DepartmentIds.Contains(c.DepartmentId.Value));
            }

            if (request.SubjectIds != null && request.SubjectIds.Any())
            {
                query = query.Where(c => c.SubjectId.HasValue && request.SubjectIds.Contains(c.SubjectId.Value));
            }

            if (request.ResponsibleUserIds != null && request.ResponsibleUserIds.Any())
            {
                query = query.Where(c => c.ResponsibleUserId.HasValue && request.ResponsibleUserIds.Contains(c.ResponsibleUserId.Value));
            }

            if (request.FollowUpUserIds != null && request.FollowUpUserIds.Any())
            {
                query = query.Where(c => c.FollowUpUserId.HasValue && request.FollowUpUserIds.Contains(c.FollowUpUserId.Value));
            }

            if (request.ClassificationIds != null && request.ClassificationIds.Any())
            {
                query = query.Where(c => c.Classifications.Any(cl => request.ClassificationIds.Contains(cl.Id)));
            }

            if (request.IsClosed.HasValue)
            {
                query = query.Where(c => c.IsClosed == request.IsClosed.Value);
            }

            // Date filters
            if (request.IncomingDateFrom.HasValue)
            {
                query = query.Where(c => c.IncomingDate >= request.IncomingDateFrom.Value);
            }
            if (request.IncomingDateTo.HasValue)
            {
                query = query.Where(c => c.IncomingDate <= request.IncomingDateTo.Value);
            }
            if (request.OutgoingDateFrom.HasValue)
            {
                query = query.Where(c => c.OutgoingDate >= request.OutgoingDateFrom.Value);
            }
            if (request.OutgoingDateTo.HasValue)
            {
                query = query.Where(c => c.OutgoingDate <= request.OutgoingDateTo.Value);
            }

            // Project to the report model
            return query.Select(c =>
                new CorrespondenceReportModel
                {
                    Id = c.Id,
                    Direction = c.Direction,
                    PriorityLevel = c.PriorityLevel,
                    IncomingNumber = c.IncomingNumber,
                    IncomingDate = c.IncomingDate,
                    OutgoingNumber = c.OutgoingNumber,
                    OutgoingDate = c.OutgoingDate,
                    CorrespondentName = c.Correspondent == null ? null : c.Correspondent.Name,
                    DepartmentName = c.Department == null ? null : c.Department.Name,
                    Subject = c.Subject == null ? null : c.Subject.Name, // Assuming Subject has a Name property
                    Summary = c.Summary,
                    ResponsibleUserName = c.ResponsibleUser == null ? null : c.ResponsibleUser.FullName, // Assuming User has a Name property
                    FollowUpUserName = c.FollowUpUser == null ? null : c.FollowUpUser.FullName, // Assuming User has a Name property
                    IsClosed = c.IsClosed,
                    Classifications = string.Join(", ", c.Classifications.Select(cl => cl.Name))
                }).AsNoTracking().ToListAsync();
        }
    }
}