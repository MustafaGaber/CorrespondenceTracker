using CorrespondenceTracker.Application.Subjects.Commands.CreateSubject;
using CorrespondenceTracker.Application.Subjects.Commands.DeleteSubject;
using CorrespondenceTracker.Application.Subjects.Commands.UpdateSubject;
using CorrespondenceTracker.Application.Subjects.Queries.GetSubjects;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class SubjectsController : BaseController
    {
        private readonly IGetSubjectsQuery _getSubjectsQuery;
        private readonly ICreateSubjectCommand _createSubjectCommand;
        private readonly IUpdateSubjectCommand _updateSubjectCommand;
        private readonly IDeleteSubjectCommand _deleteSubjectCommand;

        public SubjectsController(
            IGetSubjectsQuery getSubjectsQuery,
            ICreateSubjectCommand createSubjectCommand,
            IUpdateSubjectCommand updateSubjectCommand,
            IDeleteSubjectCommand deleteSubjectCommand)
        {
            _getSubjectsQuery = getSubjectsQuery;
            _createSubjectCommand = createSubjectCommand;
            _updateSubjectCommand = updateSubjectCommand;
            _deleteSubjectCommand = deleteSubjectCommand;
        }

        [HttpPost] // Search
        public async Task<IActionResult> Index([FromBody] GetSubjectsFilterModel request)
        {
            var result = await _getSubjectsQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            var result = await _createSubjectCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")] // Update
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] UpdateSubjectRequest request)
        {
            await _updateSubjectCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            await _deleteSubjectCommand.Execute(id);
            return Ok();
        }
    }
}