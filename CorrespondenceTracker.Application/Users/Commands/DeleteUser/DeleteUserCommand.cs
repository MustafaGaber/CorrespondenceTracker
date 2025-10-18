using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Users.Commands.DeleteUser
{
    public interface IDeleteUserCommand
    {
        Task Execute(Guid id);
    }

    public class DeleteUserCommand : IDeleteUserCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteUserCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var user = await _context.Users.FindAsync(id)
                ?? throw new ArgumentException($"User with ID {id} not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}