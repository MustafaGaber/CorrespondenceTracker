namespace CorrespondenceTracker.Application.Files.Queries.DownloadFile
{
    public interface IDownloadFileQuery
    {
        Task<DownloadFileResponse> Execute(Guid fileId);
    }
}