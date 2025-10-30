// RemindersController.cs
using CorrespondenceTracker.Application.Reminders.Commands.CreateReminder;
using CorrespondenceTracker.Application.Reminders.Commands.DeleteReminder;
using CorrespondenceTracker.Application.Reminders.Commands.UpdateReminder;
using CorrespondenceTracker.Application.Reminders.Queries.GetReminders;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class RemindersController : BaseController
    {
        private readonly ICreateReminderCommand _createCommand;
        private readonly IUpdateReminderCommand _updateCommand;
        private readonly IDeleteReminderCommand _deleteCommand;
        private readonly IGetRemindersQuery _getRemindersQuery;

        public RemindersController(
            ICreateReminderCommand createCommand,
            IUpdateReminderCommand updateCommand,
            IDeleteReminderCommand deleteCommand,
            IGetRemindersQuery getRemindersQuery)
        {
            _createCommand = createCommand;
            _updateCommand = updateCommand;
            _deleteCommand = deleteCommand;
            _getRemindersQuery = getRemindersQuery;
        }

        [HttpGet("{correspondenceId}")]
        public async Task<IActionResult> GetAll(Guid correspondenceId)
        {
            List<GetReminderResponse> result = await _getRemindersQuery.Execute(correspondenceId);
            return Ok(result);
        }

        [HttpPost("{correspondenceId}")]
        public async Task<IActionResult> Create(Guid correspondenceId, [FromBody] CreateReminderRequest request)
        {
            Guid result = await _createCommand.Execute(correspondenceId, request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReminderRequest request)
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