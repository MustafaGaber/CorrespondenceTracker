// file: CorrespondenceTracker.Application/Users/Queries/GetUsers/GetUsersQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IGetUsersQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetUsersQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<UserListDto>> Execute()
        {
            return await _context.Users
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    JobTitle = u.JobTitle
                })
                .ToListAsync();
        }
    }

    public interface IGetUsersQuery
    {
        Task<List<UserListDto>> Execute();
    }

    public class UserListDto
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string? JobTitle { get; set; }
    }
}