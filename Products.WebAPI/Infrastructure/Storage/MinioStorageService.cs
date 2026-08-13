using Minio;
using Minio.DataModel.Args;
using Products.WebAPI.Common.Enums;
using Products.WebAPI.Common.Interfaces;

namespace Products.WebAPI.Infrastructure.Storage;

public class MinioStorageService(IMinioClient client, IConfiguration config) : IFileStorageService
{
    public async Task<string> UploadFileAsync(string objectName, Stream fileStream, string contentType,
        CancellationToken cancellationToken = default)
    {
        var bucketExists = await client.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(EStorageBuckets.products.ToString()), cancellationToken);

        if (!bucketExists)
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(EStorageBuckets.products.ToString()),
                cancellationToken);

        await client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(EStorageBuckets.products.ToString())
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType), cancellationToken);
        
        return objectName;
    }

    public async Task<string> GetPresignedUrlAsync(string objectName, CancellationToken cancellationToken = default)
    {
        return await client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(EStorageBuckets.products.ToString())
            .WithObject(objectName)
            .WithExpiry(60 * 60));
    }

    public async Task DeleteAsync(string objectName, CancellationToken cancellationToken = default)
    {
        await client.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(EStorageBuckets.products.ToString())
            .WithObject(objectName), cancellationToken);
    }
}