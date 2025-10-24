using CorrespondenceTracker.Application.Subjects.Commands.CreateSubject;
using CorrespondenceTracker.Application.Subjects.Commands.DeleteSubject;
using CorrespondenceTracker.Application.Subjects.Commands.GenerateSubjectCorrespondence; // New using
using CorrespondenceTracker.Application.Subjects.Commands.UpdateSubject;
using CorrespondenceTracker.Application.Subjects.Queries.GetSubject;
using CorrespondenceTracker.Application.Subjects.Queries.GetSubjects;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class SubjectsController : BaseController
    {
        private readonly IGetSubjectsQuery _getSubjectsQuery;
        private readonly IGetSubjectQuery _getSubjectQuery;
        private readonly ICreateSubjectCommand _createSubjectCommand;
        private readonly IUpdateSubjectCommand _updateSubjectCommand;
        private readonly IDeleteSubjectCommand _deleteSubjectCommand;
        private readonly IGenerateSubjectCorrespondenceCommand _generateSubjectCorrespondenceCommand; // New field

        public SubjectsController(
            IGetSubjectsQuery getSubjectsQuery,
            ICreateSubjectCommand createSubjectCommand,
            IUpdateSubjectCommand updateSubjectCommand,
            IDeleteSubjectCommand deleteSubjectCommand,
            IGetSubjectQuery getSubjectQuery,
            IGenerateSubjectCorrespondenceCommand generateSubjectCorrespondenceCommand) // New dependency
        {
            _getSubjectsQuery = getSubjectsQuery;
            _createSubjectCommand = createSubjectCommand;
            _updateSubjectCommand = updateSubjectCommand;
            _deleteSubjectCommand = deleteSubjectCommand;
            _getSubjectQuery = getSubjectQuery;
            _generateSubjectCorrespondenceCommand = generateSubjectCorrespondenceCommand; // Initialize
        }

        // ... existing Index, GetSubject, CreateSubject, UpdateSubject, DeleteSubject actions ...

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _getSubjectsQuery.Execute();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(Guid id)
        {
            var result = await _getSubjectQuery.Execute(id);
            if (result == null)
                return NotFound(); // Return 404 if subject isn't found
            return Ok(result); // Return 200 with the response
        }

        [HttpPost]
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

        // New Endpoint for Generating Correspondence
        [HttpPost("{subjectId}/GenerateCorrespondence")]
        public async Task<IActionResult> GenerateCorrespondence(Guid subjectId,
            [FromBody] GenerateSubjectCorrespondenceRequest request)
        {
            var result = await _generateSubjectCorrespondenceCommand.Execute(subjectId, request);
            return Ok(result);
        }
    }
}