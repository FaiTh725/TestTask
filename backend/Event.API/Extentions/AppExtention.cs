using Application.Shared.Exceptions;
using Azure.Storage.Blobs;
using Event.API.Configuraions;
using StackExchange.Redis;

namespace Event.API.Extentions
{
    public static class AppExtention
    {
        public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var blobConf = configuration.GetSection("BlobStorage")
                .Get<BlobStorage>()
                ?? throw new ApplicationConfigurationException("Blob Storage");

            var connectionString = $"DefaultEndpointsProtocol=http;AccountName={blobConf.AccountName};" +
                $"AccountKey={blobConf.Key};" +
                $"BlobEndpoint={blobConf.BaseUrl}:{blobConf.Port}/{blobConf.AccountName};";

            services.AddSingleton(_ => new BlobServiceClient(connectionString));
        }

        public static void AddRedisCach(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("RedisConnection") ??
                throw new ApplicationConfigurationException("Redis Connection String");

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));
        }
    }
}
