// CreateCorrespondenceFromImageRequest.cs

using Microsoft.AspNetCore.Http;

/// <summary>
/// Represents the request to create a correspondence from an uploaded image.
/// </summary>
public class CreateCorrespondenceFromImageRequest
{
    /// <summary>
    /// The image file (e.g., PNG, JPG) containing the correspondence text.
    /// </summary>
    public IFormFile? File { get; init; } = null!;

    /// <summary>
    /// The ID of the correspondent who sent the document.
    /// </summary>
    public Guid SenderId { get; init; }
}