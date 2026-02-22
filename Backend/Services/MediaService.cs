using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Models;
using Services.Interfaces;

namespace Services;

/// <summary>
/// Service for managing media files in MinIO (S3-compatible object storage).
/// </summary>
public class MediaService : IMediaService
{
    private readonly MinioOptions _options;
    private readonly ILogger<MediaService> _logger;

    public MediaService(IOptions<MinioOptions> options, ILogger<MediaService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool IsConfigured()
    {
        return _options.IsConfigured;
    }

    /// <inheritdoc/>
    public async Task<(string Key, string Url)?> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Media storage is not properly configured");
            return null;
        }

        var minioClient = CreateMinioClient();
        if (minioClient == null)
        {
            _logger.LogError("Unable to create MinIO client");
            return null;
        }

        var uniqueObjectKey = GenerateUniqueObjectKey(fileName);

        try
        {
            await CreateBucketIfNotExistsAsync(minioClient, cancellationToken);

            var uploadRequest = new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(uniqueObjectKey)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(uploadRequest, cancellationToken);

            var accessUrl = $"/api/Media/file/{uniqueObjectKey}";
            
            _logger.LogInformation("Successfully uploaded media file: {ObjectKey} ({FileSize} bytes)", uniqueObjectKey, fileStream.Length);
            
            return (uniqueObjectKey, accessUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading media file to storage: {ObjectKey}", uniqueObjectKey);
            return null;
        }
    }

    private static string GenerateUniqueObjectKey(string originalFileName)
    {
        var sanitizedFileName = Path.GetFileName(originalFileName);
        var uniqueId = Guid.NewGuid().ToString("N");
        return $"{uniqueId}_{sanitizedFileName}";
    }

    /// <inheritdoc/>
    public async Task<(Stream Stream, string ContentType)?> GetFileAsync(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured || string.IsNullOrWhiteSpace(objectKey))
        {
            _logger.LogWarning("Cannot retrieve file: storage not configured or invalid object key");
            return null;
        }

        var minioClient = CreateMinioClient();
        if (minioClient == null)
            return null;

        try
        {
            var fileContentStream = new MemoryStream();
            var downloadRequest = new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(objectKey)
                .WithCallbackStream(stream => stream.CopyTo(fileContentStream));

            await minioClient.GetObjectAsync(downloadRequest, cancellationToken);
            fileContentStream.Position = 0;

            var mimeType = InferContentTypeFromObjectKey(objectKey);

            return (fileContentStream, mimeType);
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            _logger.LogWarning("Media file not found in storage: {ObjectKey}", objectKey);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving media file from storage: {ObjectKey}", objectKey);
            return null;
        }
    }

    private IMinioClient? CreateMinioClient()
    {
        if (!_options.IsConfigured)
            return null;

        var endpointUrl = _options.Endpoint!.Trim();
        var requiresSecureConnection = _options.UseSSL || endpointUrl.StartsWith("https:", StringComparison.OrdinalIgnoreCase);
        var endpointHost = endpointUrl
            .Replace("https://", "", StringComparison.OrdinalIgnoreCase)
            .Replace("http://", "", StringComparison.OrdinalIgnoreCase)
            .TrimEnd('/');

        return new MinioClient()
            .WithEndpoint(endpointHost)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(requiresSecureConnection)
            .Build();
    }

    private async Task CreateBucketIfNotExistsAsync(IMinioClient client, CancellationToken cancellationToken)
    {
        var bucketCheckRequest = new BucketExistsArgs().WithBucket(_options.BucketName);
        var bucketExists = await client.BucketExistsAsync(bucketCheckRequest, cancellationToken);
        
        if (bucketExists)
            return;

        var createBucketRequest = new MakeBucketArgs().WithBucket(_options.BucketName);
        await client.MakeBucketAsync(createBucketRequest, cancellationToken);
        _logger.LogInformation("Created media storage bucket: {BucketName}", _options.BucketName);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFileAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured || string.IsNullOrWhiteSpace(objectKey))
        {
            _logger.LogWarning("Cannot delete file: storage not configured or invalid object key");
            return false;
        }

        var minioClient = CreateMinioClient();
        if (minioClient == null)
            return false;

        try
        {
            var deleteRequest = new RemoveObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(objectKey);

            await minioClient.RemoveObjectAsync(deleteRequest, cancellationToken);
            
            _logger.LogInformation("Successfully deleted media file: {ObjectKey}", objectKey);
            return true;
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            _logger.LogWarning("File not found for deletion: {ObjectKey}", objectKey);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting media file from storage: {ObjectKey}", objectKey);
            return false;
        }
    }

    private static string InferContentTypeFromObjectKey(string objectKey)
    {
        var lowerKey = objectKey.ToLowerInvariant();
        
        if (lowerKey.EndsWith(".jpg") || lowerKey.EndsWith(".jpeg"))
            return "image/jpeg";
        
        if (lowerKey.EndsWith(".png"))
            return "image/png";
        
        if (lowerKey.EndsWith(".gif"))
            return "image/gif";
        
        if (lowerKey.EndsWith(".webp"))
            return "image/webp";

        return "application/octet-stream";
    }
}
