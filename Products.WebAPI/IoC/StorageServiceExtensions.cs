using Minio;
using Products.WebAPI.Common.Interfaces;
using Products.WebAPI.Infrastructure.Storage;

namespace Products.WebAPI.IoC;

public static class StorageServiceExtensions
{
    public static IServiceCollection AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMinioClient>(_ =>
            new MinioClient()
                .WithEndpoint(configuration["Minio:Endpoint"])
                .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
                .WithSSL(bool.Parse(configuration["Minio:UseSSL"] ?? "false"))
                .Build());

        services.AddScoped<IFileStorageService, MinioStorageService>();
        
        return services;
    }
}