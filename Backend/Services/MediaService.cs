using Microsoft.Extensions.Options;
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

    public MediaService(IOptions<MinioOptions> options)
    {
        _options = options.Value;
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
            return null;
        }

        var minioClient = CreateMinioClient();
        if (minioClient == null)
        {
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
                        
            return (uniqueObjectKey, accessUrl);
        }
        catch (Exception ex)
        {
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
            return null;
        }
        catch (Exception ex)
        {
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
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFileAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured || string.IsNullOrWhiteSpace(objectKey))
        {
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
            
            return true;
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return false;
        }
        catch (Exception ex)
        {
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
