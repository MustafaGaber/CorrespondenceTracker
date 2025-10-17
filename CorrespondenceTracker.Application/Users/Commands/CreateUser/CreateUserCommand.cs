// file: CorrespondenceTracker.Application/Users/Commands/CreateUser/CreateUserCommand.cs
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICreateUserCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateUserCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateUserRequest model)
        {
            var user = new User(model.FullName, model.JobTitle);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }

    public interface ICreateUserCommand
    {
        Task<Guid> Execute(CreateUserRequest model);
    }

    public class CreateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
    }
} 