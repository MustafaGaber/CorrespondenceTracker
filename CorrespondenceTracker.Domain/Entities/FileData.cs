namespace CorrespondenceTracker.Domain.Entities
{
    public class FileData
    {
        public required string FullPath { get; init; }
        public required string Extension { get; init; }
        public required long Size { get; init; }
    }
}
