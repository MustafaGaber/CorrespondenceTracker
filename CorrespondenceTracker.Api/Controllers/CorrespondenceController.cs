using CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondence;
using CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondenceFromImage;
using CorrespondenceTracker.Application.Correspondences.Commands.DeleteCorrespondence;
using CorrespondenceTracker.Application.Correspondences.Commands.UpdateCorrespondence;
using CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondence;
using CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class CorrespondenceController : BaseController
    {
        private readonly IGetCorrespondenceQuery _getCorrespondenceQuery;
        private readonly IGetCorrespondencesQuery _getCorrespondencesQuery;
        private readonly ICreateCorrespondenceCommand _createCorrespondenceCommand;
        private readonly IUpdateCorrespondenceCommand _updateCorrespondenceCommand;
        private readonly IDeleteCorrespondenceCommand _deleteCorrespondenceCommand;
        private readonly ICreateCorrespondenceFromImageCommand _createFromImageCommand; // New

        public CorrespondenceController(
            IGetCorrespondencesQuery getCorrespondencesQuery,
            ICreateCorrespondenceCommand createCorrespondenceCommand,
            IUpdateCorrespondenceCommand updateCorrespondenceCommand,
            IDeleteCorrespondenceCommand deleteCorrespondenceCommand,
            ICreateCorrespondenceFromImageCommand createFromImageCommand // New
,
            IGetCorrespondenceQuery getCorrespondenceQuery)
        {
            _getCorrespondencesQuery = getCorrespondencesQuery;
            _createCorrespondenceCommand = createCorrespondenceCommand;
            _updateCorrespondenceCommand = updateCorrespondenceCommand;
            _deleteCorrespondenceCommand = deleteCorrespondenceCommand;
            _createFromImageCommand = createFromImageCommand; // New
            _getCorrespondenceQuery = getCorrespondenceQuery;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCorrespondence(Guid id)
        {
            var result = await _getCorrespondenceQuery.Execute(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] GetCorrespondencesFilterModel request)
        {
            var result = await _getCorrespondencesQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCorrespondence([FromForm] CreateCorrespondenceRequest request)
        {
            var result = await _createCorrespondenceCommand.Execute(request);
            return Ok(result);
        }

        // New endpoint for creating from an image
        [HttpPost("CreateFromImage")]
        public async Task<IActionResult> CreateFromImage([FromForm] CreateCorrespondenceFromImageRequest request)
        {
            var result = await _createFromImageCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCorrespondence(Guid id, [FromForm] CreateCorrespondenceRequest request)
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
