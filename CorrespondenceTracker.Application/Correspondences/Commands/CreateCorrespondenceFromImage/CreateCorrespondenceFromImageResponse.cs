// CreateCorrespondenceFromImageResponse.cs

public class CreateCorrespondenceFromImageResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public Guid? FileId { get; set; }
}