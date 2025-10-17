// file: CorrespondenceTracker.Application/Users/Queries/GetUser/GetUserQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IGetUserQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetUserQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> Execute(Guid id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    JobTitle = u.JobTitle,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }
    }

    public interface IGetUserQuery
    {
        Task<UserDto?> Execute(Guid id);
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}