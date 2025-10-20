namespace CorrespondenceTracker.Application.Files.Queries.DownloadFile
{
    public class DownloadFileResponse
    {
        public required MemoryStream Stream { get; init; }
        public required string Name { get; init; }
    }
}
