using CorrespondenceTracker.Application.FollowUps.Commands.CreateFollowUp;
using CorrespondenceTracker.Application.FollowUps.Commands.DeleteFollowUp;
using CorrespondenceTracker.Application.FollowUps.Commands.UpdateFollowUp;
using CorrespondenceTracker.Application.FollowUps.Queries.GetFollowUp;
using CorrespondenceTracker.Application.FollowUps.Queries.GetFollowUps;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class FollowUpsController : BaseController
    {
        private readonly ICreateFollowUpCommand _createCommand;
        private readonly IUpdateFollowUpCommand _updateCommand;
        private readonly IDeleteFollowUpCommand _deleteCommand;
        private readonly IGetFollowUpsQuery _getFollowUpsQuery;
        private readonly IGetFollowUpQuery _getFollowUpQuery;

        public FollowUpsController(
            ICreateFollowUpCommand createCommand,
            IUpdateFollowUpCommand updateCommand,
            IDeleteFollowUpCommand deleteCommand,
            IGetFollowUpsQuery getFollowUpsQuery,
            IGetFollowUpQuery getFollowUpQuery)
        {
            _createCommand = createCommand;
            _updateCommand = updateCommand;
            _deleteCommand = deleteCommand;
            _getFollowUpsQuery = getFollowUpsQuery;
            _getFollowUpQuery = getFollowUpQuery;
        }

        [HttpGet("{correspondenceId}")]
        public async Task<IActionResult> GetAll(Guid correspondenceId)
        {
            List<GetFollowUpResponse> result = await _getFollowUpsQuery.Execute(correspondenceId);
            return Ok(result);
        }

        [HttpGet("{correspondenceId}/{id}")]
        public async Task<IActionResult> Get(Guid correspondenceId, Guid id)
        {
            GetFollowUpDetailResponse result = await _getFollowUpQuery.Execute(id);
            return Ok(result);
        }

        [HttpPost("{correspondenceId}")]
        public async Task<IActionResult> Create(Guid correspondenceId, [FromBody] CreateFollowUpRequest request)
        {
            Guid result = await _createCommand.Execute(correspondenceId, request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFollowUpRequest request)
        {
            await _updateCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteCommand.Execute(id);
            return NoContent();
        }
    }
}