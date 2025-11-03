// CorrespondencesReportController.cs
using CorrespondenceTracker.Application.Reports.Queries.GetCorrespondencesReportData; // Update namespace
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;

namespace NUCA.Api.Controllers.CorrespondencesApi.Reports // Updated route
{
    [Route("CorrespondenceApi/[controller]")] // Updated route
    [ApiController]
    public class CorrespondencesReportController : Controller
    {
        private readonly IGetCorrespondenceReportDataQuery _getCorrespondencesReportQuery;
        private readonly IJsReportMVCService _jsReportMVCService;

        public CorrespondencesReportController(
            IGetCorrespondenceReportDataQuery getCorrespondencesReportQuery,
            IJsReportMVCService jsReportMVCService)
        {
            _getCorrespondencesReportQuery = getCorrespondencesReportQuery;
            _jsReportMVCService = jsReportMVCService;
        }

        [HttpPost("Pdf")]
        public async Task<IActionResult> GetPdfReport([FromBody] GetCorrespondencesReportRequest request)
        {
            List<CorrespondenceReportModel> correspondences = await _getCorrespondencesReportQuery.Execute(request, User);

            // This part for the header is copied directly from your ProjectsReportController
            var content = await _jsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "CorrespondencesReport", (correspondences, request));
            //var header = await _jsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Header", new { });

            // Copied from ProjectsReportController to maintain the exact style
            var report = await _jsReportMVCService.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = content,
                    Chrome = new Chrome()
                    {
                        Format = "A4",
                        Landscape = true,
                        DisplayHeaderFooter = true,
                        HeaderTemplate = "<div></div>", //header,
                        FooterTemplate = "<div></div>",
                        MarginTop = "2cm",
                        MarginBottom = "3cm",
                        MarginLeft = "1.8cm",
                        MarginRight = "1.5cm",
                    },
                },
            });

            var memoryStream = new MemoryStream();
            await report.Content.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/pdf");
        }
    }
}