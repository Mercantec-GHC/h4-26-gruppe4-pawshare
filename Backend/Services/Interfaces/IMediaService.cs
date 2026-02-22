namespace Services.Interfaces;

/// <summary>
/// Service interface for managing media files in MinIO storage.
/// </summary>
public interface IMediaService
{
    /// <summary>
    /// Uploads a file to MinIO storage.
    /// </summary>
    /// <param name="fileStream">The file stream to upload.</param>
    /// <param name="fileName">The original filename.</param>
    /// <param name="contentType">The content type of the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The object key and URL if successful, otherwise null.</returns>
    Task<(string Key, string Url)?> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a file from MinIO storage.
    /// </summary>
    /// <param name="objectKey">The object key (file identifier).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The file stream and content type if found, otherwise null.</returns>
    Task<(Stream Stream, string ContentType)?> GetFileAsync(
        string objectKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from MinIO storage.
    /// </summary>
    /// <param name="objectKey">The object key (file identifier) to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the file was deleted successfully, false otherwise.</returns>
    Task<bool> DeleteFileAsync(string objectKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if MinIO is configured and available.
    /// </summary>
    /// <returns>True if MinIO is configured, otherwise false.</returns>
    bool IsConfigured();
}
