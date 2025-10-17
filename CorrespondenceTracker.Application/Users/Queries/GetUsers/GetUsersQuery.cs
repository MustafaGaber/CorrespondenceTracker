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
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
    }
}