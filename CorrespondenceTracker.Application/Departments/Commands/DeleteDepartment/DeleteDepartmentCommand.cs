// Department/Commands/DeleteDepartmentCommand.cs
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IDeleteDepartmentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public DeleteDepartmentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                throw new ArgumentException($"Department with ID {id} not found");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

    public interface IDeleteDepartmentCommand
    {
        Task Execute(Guid id);
    }
}