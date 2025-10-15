using CorrespondenceTracker.Application.Correspondents.Queries.GetCorrespondents;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class CorrespondentsController : BaseController
    {
        private readonly IGetCorrespondentsQuery _getCorrespondentsQuery;

        public CorrespondentsController(IGetCorrespondentsQuery getCorrespondentsQuery)
        {
            _getCorrespondentsQuery = getCorrespondentsQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetCorrespondents([FromQuery] string? search = null)
        {
            var result = await _getCorrespondentsQuery.Execute(search);
            return Ok(result);
        }
    }
}
