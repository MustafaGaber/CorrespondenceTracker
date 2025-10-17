// User/Commands/UpdateUserCommand.cs
using CorrespondenceTracker.Application.Users.Commands.CreateUser;
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new ArgumentException($"User with ID {id} not found");

            // Since User doesn't have an Update method, we'll update properties directly
            // In a real scenario, you'd want to add an Update method to the User entity
            var updatedUser = new User(model.FullName, model.JobTitle) { Id = id };
            _context.Entry(user).CurrentValues.SetValues(updatedUser);
            await _context.SaveChangesAsync();
        }
    }

    public interface IUpdateUserCommand
    {
        Task Execute(Guid id, CreateUserRequest model);
    }
}