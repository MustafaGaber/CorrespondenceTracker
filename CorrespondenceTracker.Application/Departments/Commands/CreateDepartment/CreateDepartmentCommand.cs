// Department/Commands/CreateDepartmentCommand.cs
using CorrespondenceTracker.Data;
using CorrespondenceTracker.Domain.Entities;

namespace CorrespondenceTracker.Application.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : ICreateDepartmentCommand
    {
        private readonly CorrespondenceDatabaseContext _context;

        public CreateDepartmentCommand(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Execute(CreateDepartmentRequest model)
        {
            var department = new Department(model.Name);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department.Id;
        }
    }

    public interface ICreateDepartmentCommand
    {
        Task<Guid> Execute(CreateDepartmentRequest model);
    }

    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}