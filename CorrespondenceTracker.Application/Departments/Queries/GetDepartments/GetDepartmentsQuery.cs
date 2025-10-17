// Department/Queries/GetDepartmentsQuery.cs
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CorrespondenceTracker.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentsQuery : IGetDepartmentsQuery
    {
        private readonly CorrespondenceDatabaseContext _context;

        public GetDepartmentsQuery(CorrespondenceDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<GetDepartmentResponse>> Execute()
        {


            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();

            return departments.Select(d => new GetDepartmentResponse
            {
                Id = d.Id,
                Name = d.Name,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList();
        }

        public async Task<GetDepartmentResponse?> GetById(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;

            return new GetDepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            };
        }
    }

    public interface IGetDepartmentsQuery
    {
        Task<List<GetDepartmentResponse>> Execute();
        Task<GetDepartmentResponse?> GetById(Guid id);
    }
}