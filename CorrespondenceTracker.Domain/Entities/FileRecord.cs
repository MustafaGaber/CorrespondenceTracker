// FileRecord.cs
using Ardalis.GuardClauses;

namespace CorrespondenceTracker.Domain.Entities
{
    public class FileRecord : Entity
    {
        public string FileName { get; private set; } = string.Empty;
        public string RelativePath { get; private set; } = string.Empty;
        public string ContentType { get; private set; } = string.Empty;
        public string Extension { get; private set; } = string.Empty;
        public long Size { get; private set; }

        // Protected constructor for ORM
        protected FileRecord() { }

        // Public constructor
        public FileRecord(string fileName, string relativePath, string contentType, string extension, long size)
        {
            FileName = Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            RelativePath = Guard.Against.NullOrWhiteSpace(relativePath, nameof(relativePath));
            ContentType = Guard.Against.NullOrWhiteSpace(contentType, nameof(contentType));
            Extension = Guard.Against.NullOrWhiteSpace(extension, nameof(extension));
            Size = Guard.Against.NegativeOrZero(size, nameof(size));
        }
    }
}