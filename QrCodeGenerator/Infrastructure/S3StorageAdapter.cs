using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using QrCodeGenerator.Contracts;
using QrCodeGenerator.DTO;

namespace QrCodeGenerator.Infrastructure;

public class S3StorageAdapter(IOptions<S3Options> options, ILogger<S3StorageAdapter> logger)
    : IStoragePort
{
    private readonly AmazonS3Client _s3Client = new();
    private readonly string _bucketName = options.Value.BucketName ?? string.Empty;
    private readonly string _region = options.Value.Region ?? string.Empty;

    public async Task<string> UploadFileAsync(Stream fileData, string fileName, string contentType)
    {
        if (!await CreateBucket()) 
            return "";
        
        var request = new PutObjectRequest()
            { BucketName = _bucketName, Key = fileName, ContentType = contentType, InputStream = fileData };
        await _s3Client.PutObjectAsync(request);
        return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileName}";
    }

    private async Task<bool> CreateBucket()
    {
        try
        {
            var result = await _s3Client.PutBucketAsync(new PutBucketRequest()
                { BucketName = _bucketName, BucketRegionName = _region});
            return result.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            logger.LogError(ex, "Failed to create bucket");
            throw;
        }
    }
}