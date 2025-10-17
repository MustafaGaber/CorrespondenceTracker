using CorrespondenceTracker.Application.Correspondents.Commands.CreateCorrespondent;
using CorrespondenceTracker.Application.Correspondents.Commands.DeleteCorrespondent;
using CorrespondenceTracker.Application.Correspondents.Commands.UpdateCorrespondent;
using CorrespondenceTracker.Application.Correspondents.Queries.GetCorrespondents;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class CorrespondentsController : BaseController
    {
        private readonly IGetCorrespondentsQuery _getCorrespondentsQuery;
        private readonly ICreateCorrespondentCommand _createCorrespondentCommand;
        private readonly IUpdateCorrespondentCommand _updateCorrespondentCommand;
        private readonly IDeleteCorrespondentCommand _deleteCorrespondentCommand;

        public CorrespondentsController(
            IGetCorrespondentsQuery getCorrespondentsQuery,
            ICreateCorrespondentCommand createCorrespondentCommand,
            IUpdateCorrespondentCommand updateCorrespondentCommand,
            IDeleteCorrespondentCommand deleteCorrespondentCommand)
        {
            _getCorrespondentsQuery = getCorrespondentsQuery;
            _createCorrespondentCommand = createCorrespondentCommand;
            _updateCorrespondentCommand = updateCorrespondentCommand;
            _deleteCorrespondentCommand = deleteCorrespondentCommand;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _getCorrespondentsQuery.Execute();
            return Ok(result);
        }

        [HttpPost("Search")] // Search
        public async Task<IActionResult> Search([FromBody] GetCorrespondentsFilterModel request)
        {
            var result = await _getCorrespondentsQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCorrespondent([FromBody] CreateCorrespondentRequest request)
        {
            var result = await _createCorrespondentCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")] // Update
        public async Task<IActionResult> UpdateCorrespondent(Guid id, [FromBody] CreateCorrespondentRequest request)
        {
            await _updateCorrespondentCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrespondent(Guid id)
        {
            await _deleteCorrespondentCommand.Execute(id);
            return Ok();
        }
    }
}
