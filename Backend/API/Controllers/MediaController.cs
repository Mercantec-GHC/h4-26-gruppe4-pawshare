using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

/// <summary>
/// Controller for managing media file uploads and retrieval from MinIO storage.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    /// <summary>
    /// Uploads a file to MinIO storage.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The object key and URL if successful.</returns>
    /// <response code="200">Returns the upload response with key and URL.</response>
    /// <response code="400">If no file was provided.</response>
    /// <response code="503">If MinIO is not configured.</response>
    /// <response code="500">If the upload failed.</response>
    [HttpPost("upload")]
    [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB
    public async Task<ActionResult<MediaUploadResponse>> Upload(IFormFile? file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided" });

        if (!_mediaService.IsConfigured())
            return StatusCode(503, new { error = "Media storage service is not available" });

        var fileExtension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
        var detectedContentType = DetermineContentType(file.ContentType, fileExtension);

        using var fileStream = file.OpenReadStream();
        var uploadResult = await _mediaService.UploadFileAsync(fileStream, file.FileName, detectedContentType, cancellationToken);

        if (uploadResult == null)
            return StatusCode(500, new { error = "File upload failed" });

        var fileUrl = Url.Action(nameof(GetFile), "Media", new { key = uploadResult.Value.Key }, Request.Scheme, Request.Host.Value) ?? "";
        
        return Ok(new MediaUploadResponse 
        { 
            ObjectKey = uploadResult.Value.Key, 
            FileUrl = fileUrl 
        });
    }

    private static string DetermineContentType(string? providedContentType, string extension)
    {
        if (!string.IsNullOrWhiteSpace(providedContentType))
            return providedContentType;

        return extension switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    /// Retrieves a file from MinIO storage by its object key.
    /// </summary>
    /// <param name="key">The object key (file identifier).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The file content.</returns>
    /// <response code="200">Returns the file content.</response>
    /// <response code="400">If the key is invalid.</response>
    /// <response code="404">If the file is not found.</response>
    /// <response code="503">If MinIO is not configured.</response>
    [HttpGet("file/{*key}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFile(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest(new { error = "Invalid file identifier" });

        if (!_mediaService.IsConfigured())
            return StatusCode(503, new { error = "Media storage service is not available" });

        var fileResult = await _mediaService.GetFileAsync(key, cancellationToken);

        if (fileResult == null)
            return NotFound();

        Response.Headers.CacheControl = "public, max-age=3600";
        return File(fileResult.Value.Stream, fileResult.Value.ContentType);
    }
}

/// <summary>
/// Response DTO for successful media file uploads.
/// </summary>
public class MediaUploadResponse
{
    /// <summary>
    /// The unique object key identifier for the uploaded file in storage.
    /// </summary>
    public string ObjectKey { get; set; } = string.Empty;

    /// <summary>
    /// The public URL where the uploaded file can be accessed.
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;
}