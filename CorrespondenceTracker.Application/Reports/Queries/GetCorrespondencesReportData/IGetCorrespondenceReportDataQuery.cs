// IGetCorrespondenceReportDataQuery.cs
using System.Security.Claims;

namespace CorrespondenceTracker.Application.Reports.Queries.GetCorrespondencesReportData
{
    public interface IGetCorrespondenceReportDataQuery
    {
        Task<List<CorrespondenceReportModel>> Execute(GetCorrespondencesReportRequest request, ClaimsPrincipal user);
    }
}