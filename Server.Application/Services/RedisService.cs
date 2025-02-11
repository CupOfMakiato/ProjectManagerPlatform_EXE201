using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Server.Application.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConfiguration configuration)
        {
            // Retrieve the Redis connection string from the environment variable
            //var connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
            //                       ?? configuration["ConnectionStrings:Redis"]; // Fallback to appsettings if env var is missing
            var connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
                                   ?? configuration["Redis:ConnectionString"];
            // Connect to Redis using the connection string
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();

        }

        public async Task SetStringAsync(string key, string value, TimeSpan expiry)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}
