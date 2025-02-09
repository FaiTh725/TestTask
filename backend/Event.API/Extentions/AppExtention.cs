using Application.Shared.Exceptions;
using Azure.Storage.Blobs;
using Event.API.Configuraions;
using Event.API.Contracts.Event;
using Event.API.Contracts.Member;
using Event.API.Validators.Event;
using Event.API.Validators.Member;
using Event.Application.Models.Events;
using FluentValidation;
using FluentValidation.AspNetCore;
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

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "Events";
            });

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateEventRequest>, CreateEventValidator>();
            services.AddScoped<IValidator<UpdateEventRequest>, UpdateEventValidator>();
            services.AddScoped<IValidator<CreateMemberRequest>, CreateMemberValidator>();
        }
    }
}
