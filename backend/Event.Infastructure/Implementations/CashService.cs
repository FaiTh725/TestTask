using CSharpFunctionalExtensions;
using Event.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Event.Infastructure.Implementations
{
    public class CashService : ICachService
    {
        private readonly IDistributedCache cache;
        private readonly IServer server;

        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public CashService(
            IDistributedCache cache,
            IConnectionMultiplexer connection)
        {
            var endpoint = connection.GetEndPoints();
            server = connection.GetServer(endpoint[0]);

            this.cache = cache; 
        }

        public async Task<Result<T>> GetData<T>(string key)
        {
            var jsonData = await cache.GetStringAsync(key);
        
            if(jsonData is null)
            {
                return Result.Failure<T>("Value Doesnt Set");
            }

            var data = JsonSerializer.Deserialize<T>(
                jsonData, serializerOptions);
        
            if(data is null)
            {
                return Result.Failure<T>("Deserialize Error");
            }

            return Result.Success(data);
        }

        public async Task RemoveData(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T value, int durationSeconds = 120)
        {
            var cachOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(durationSeconds)
            };

            var jsonData = JsonSerializer.Serialize(value);

            await cache.SetStringAsync(key, jsonData, cachOptions);
        }

        // If I use this method, I will only get data from the cache 
        public async Task<IEnumerable<T>> GetDataFolder<T>(string folderPattern)
        {
            var redisKeys = server.Keys(pattern: folderPattern).ToList();

            var tasks = new List<Task<Result<T>>>();

            foreach(var key in redisKeys)
            {
                tasks.Add(GetData<T>(key));
            }

            var values = await Task.WhenAll(tasks);

            var data = new List<T>();

            foreach(var value in values)
            {
                if(value.IsSuccess)
                {
                    data.Add(value.Value);
                }
            }

            return data;
        }
    }
}
