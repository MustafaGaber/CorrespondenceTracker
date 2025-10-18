using CorrespondenceTracker.Application.Departments.Commands.CreateDepartment;
using CorrespondenceTracker.Application.Departments.Commands.DeleteDepartment;
using CorrespondenceTracker.Application.Departments.Commands.UpdateDepartment;
using CorrespondenceTracker.Application.Departments.Queries.GetDepartments;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        private readonly IGetDepartmentsQuery _getDepartmentsQuery;
        private readonly ICreateDepartmentCommand _createDepartmentCommand;
        private readonly IUpdateDepartmentCommand _updateDepartmentCommand;
        private readonly IDeleteDepartmentCommand _deleteDepartmentCommand;

        public DepartmentsController(
            IGetDepartmentsQuery getDepartmentsQuery,
            ICreateDepartmentCommand createDepartmentCommand,
            IUpdateDepartmentCommand updateDepartmentCommand,
            IDeleteDepartmentCommand deleteDepartmentCommand)
        {
            _getDepartmentsQuery = getDepartmentsQuery;
            _createDepartmentCommand = createDepartmentCommand;
            _updateDepartmentCommand = updateDepartmentCommand;
            _deleteDepartmentCommand = deleteDepartmentCommand;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _getDepartmentsQuery.Execute();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var result = await _getDepartmentsQuery.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var result = await _createDepartmentCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")] // Update
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] CreateDepartmentRequest request)
        {
            await _updateDepartmentCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            await _deleteDepartmentCommand.Execute(id);
            return Ok();
        }
    }
}