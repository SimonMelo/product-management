namespace Products.WebAPI.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string objectName, Stream fileStream, string contentType, CancellationToken cancellationToken);
    Task<string> GetPresignedUrlAsync(string objectName, CancellationToken cancellationToken);
    Task DeleteAsync(string objectName, CancellationToken cancellationToken);
}