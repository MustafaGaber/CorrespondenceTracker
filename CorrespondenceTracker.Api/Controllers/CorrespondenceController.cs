// CorrespondenceController.cs
using CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondence;
using CorrespondenceTracker.Application.Correspondences.Commands.DeleteCorrespondence; // New
using CorrespondenceTracker.Application.Correspondences.Commands.UpdateCorrespondence; // New
using CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class CorrespondenceController : BaseController
    {

        private readonly IGetCorrespondencesQuery _getCorrespondencesQuery;
        private readonly ICreateCorrespondenceCommand _createCorrespondenceCommand;
        private readonly IUpdateCorrespondenceCommand _updateCorrespondenceCommand; // New
        private readonly IDeleteCorrespondenceCommand _deleteCorrespondenceCommand; // New

        public CorrespondenceController(
            IGetCorrespondencesQuery getCorrespondencesQuery,
            ICreateCorrespondenceCommand createCorrespondenceCommand,
            IUpdateCorrespondenceCommand updateCorrespondenceCommand,
            IDeleteCorrespondenceCommand deleteCorrespondenceCommand
        )
        {
            _getCorrespondencesQuery = getCorrespondencesQuery;
            _createCorrespondenceCommand = createCorrespondenceCommand;
            _updateCorrespondenceCommand = updateCorrespondenceCommand;
            _deleteCorrespondenceCommand = deleteCorrespondenceCommand;
        }

        [HttpPost] // Search
        public async Task<IActionResult> Index([FromBody] GetCorrespondencesFilterModel request)
        {
            var result = await _getCorrespondencesQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCorrespondence([FromBody] CreateCorrespondenceRequest request)
        {
            var result = await _createCorrespondenceCommand.Execute(request);
            return Ok(result);
        }


        [HttpPut("{id}")] // Update
        public async Task<IActionResult> UpdateCorrespondence(Guid id, [FromBody] CreateCorrespondenceRequest request)
        {
            await _updateCorrespondenceCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrespondence(Guid id)
        {
            await _deleteCorrespondenceCommand.Execute(id);
            return Ok();
        }

    }
}