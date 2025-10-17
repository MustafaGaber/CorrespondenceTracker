// Department/Queries/GetDepartmentsQuery.cs
namespace CorrespondenceTracker.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}