// User/Commands/UpdateUserCommand.cs
using CorrespondenceTracker.Application.Users.Commands.CreateUser;
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IUpdateUserCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateUserCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, CreateUserRequest model)
        {
            var user = await _context.Users.FindAsync(id)
                ?? throw new ArgumentException($"User with ID {id} not found");

            user.Update(model.FullName, model.Email, model.JobTitle);
            await _context.SaveChangesAsync();
        }
    }

    public interface IUpdateUserCommand
    {
        Task Execute(Guid id, CreateUserRequest model);
    }
}