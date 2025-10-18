using CorrespondenceTracker.Application.Classifications.Commands.CreateClassification;
using CorrespondenceTracker.Application.Classifications.Commands.DeleteClassification;
using CorrespondenceTracker.Application.Classifications.Commands.UpdateClassification;
using CorrespondenceTracker.Application.Classifications.Queries.GetClassifications;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class ClassificationsController : BaseController
    {
        private readonly IGetClassificationsQuery _getClassificationsQuery;
        private readonly ICreateClassificationCommand _createClassificationCommand;
        private readonly IUpdateClassificationCommand _updateClassificationCommand;
        private readonly IDeleteClassificationCommand _deleteClassificationCommand;

        public ClassificationsController(
            IGetClassificationsQuery getClassificationsQuery,
            ICreateClassificationCommand createClassificationCommand,
            IUpdateClassificationCommand updateClassificationCommand,
            IDeleteClassificationCommand deleteClassificationCommand)
        {
            _getClassificationsQuery = getClassificationsQuery;
            _createClassificationCommand = createClassificationCommand;
            _updateClassificationCommand = updateClassificationCommand;
            _deleteClassificationCommand = deleteClassificationCommand;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _getClassificationsQuery.Execute();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassification([FromBody] CreateClassificationRequest request)
        {
            var result = await _createClassificationCommand.Execute(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassification(Guid id, [FromBody] UpdateClassificationRequest request)
        {
            await _updateClassificationCommand.Execute(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassification(Guid id)
        {
            await _deleteClassificationCommand.Execute(id);
            return Ok();
        }
    }
}