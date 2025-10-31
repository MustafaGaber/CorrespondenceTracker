using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondence
{
    public class GetCorrespondenceQuery : IGetCorrespondenceQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetCorrespondenceQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<GetCorrespondenceResponse?> Execute(Guid id)
        {
            var correspondence = await _context.Correspondences
                .Include(l => l.Correspondent)
                .Include(l => l.Department)
                .Include(l => l.FollowUpUser)
                .Include(l => l.ResponsibleUser)
                .Include(l => l.Classifications)
                .Include(l => l.Subject)
                .Include(l => l.File)
                // Include FollowUps and the associated User for each follow-up
                .Include(l => l.FollowUps)
                    .ThenInclude(f => f.User)
                // Include Reminders
                .Include(l => l.Reminders)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (correspondence == null)
                return null;

            return new GetCorrespondenceResponse
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
                Content = correspondence.Content,
                FollowUpUser = correspondence.FollowUpUser != null ? new UserDto
                {
                    Id = correspondence.FollowUpUser.Id,
                    Name = correspondence.FollowUpUser.FullName
                } : null,
                ResponsibleUser = correspondence.ResponsibleUser != null ? new UserDto
                {
                    Id = correspondence.ResponsibleUser.Id,
                    Name = correspondence.ResponsibleUser.FullName
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
                }).ToList(),
                // Map Follow-Ups to DTOs
                FollowUps = correspondence.FollowUps.Select(f => new FollowUpDto
                {
                    Id = f.Id,
                    FileRecordId = f.FileRecordId,
                    Date = f.Date,
                    Details = f.Details,
                    User = f.User != null ? new UserDto
                    {
                        Id = f.User.Id,
                        Name = f.User.FullName
                    } : null
                }).ToList(),
                // Map Reminders to DTOs
                Reminders = correspondence.Reminders.Select(r => new ReminderDto
                {
                    Id = r.Id,
                    RemindTime = r.RemindTime,
                    Message = r.Message,
                    SendEmailMessage = r.SendEmailMessage,
                    IsCompleted = r.IsCompleted,
                    IsDismissed = r.IsDismissed,
                    CompletedAt = r.CompletedAt,
                    IsActive = r.IsActive,
                    IsOverdue = r.IsOverdue
                }).ToList()
            };
        }
    }

    public interface IGetCorrespondenceQuery
    {
        Task<GetCorrespondenceResponse?> Execute(Guid id);
    }
}