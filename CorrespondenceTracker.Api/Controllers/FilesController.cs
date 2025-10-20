using CorrespondenceTracker.Application.Files.Queries.DownloadFile;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceTracker.Api.Controllers
{
    [Route("CorrespondenceApi/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IDownloadFileQuery _downloadFileQuery;
        public FilesController(IDownloadFileQuery downloadFileQuery)
        {
            _downloadFileQuery = downloadFileQuery;
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var file = await _downloadFileQuery.Execute(fileId);
            return File(file.Stream, "application/octet-stream", file.Name, true);
        }


    }
}