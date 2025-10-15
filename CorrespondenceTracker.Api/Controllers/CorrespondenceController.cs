using CorrespondenceTracker.Application.Correspondences.Commands.CreateCorrespondence;
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

        public CorrespondenceController(IGetCorrespondencesQuery getCorrespondencesQuery, ICreateCorrespondenceCommand createCorrespondenceCommand)
        {
            _getCorrespondencesQuery = getCorrespondencesQuery;
            _createCorrespondenceCommand = createCorrespondenceCommand;
        }

        [HttpPost]
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

    }
}
