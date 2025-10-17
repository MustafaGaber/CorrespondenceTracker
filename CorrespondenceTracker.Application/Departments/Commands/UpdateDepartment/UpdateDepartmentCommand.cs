// Department/Commands/UpdateDepartmentCommand.cs
using CorrespondenceTracker.Application.Departments.Commands.CreateDepartment;
using CorrespondenceTracker.Data;

namespace CorrespondenceTracker.Application.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IUpdateDepartmentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public UpdateDepartmentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task Execute(Guid id, CreateDepartmentRequest model)
        {
            var department = await _context.Departments.FindAsync(id) ?? throw new ArgumentException($"Department with ID {id} not found");
            department.Update(model.Name);
            await _context.SaveChangesAsync();
        }
    }

    public interface IUpdateDepartmentCommand
    {
        Task Execute(Guid id, CreateDepartmentRequest model);
    }
}