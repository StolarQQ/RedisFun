using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisCore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var redisService = new Redis();
            await redisService.SetCache("name", "Andrew");

            var data = await redisService.GetCache("name");

            Console.WriteLine($"Fetching data from redis cache '{data}'.");
            Console.ReadKey();
        }
    }

    public interface IRedis
    {
        Task<string> GetCache(string key);
        Task SetCache(string key, string value);
    }

    public class Redis : IRedis
    {
        private readonly IDatabase _database;

        public Redis()
        {
            // Connect to docker container - default machine win 7 
            var redis = ConnectionMultiplexer.Connect("192.168.99.100");
            _database = redis.GetDatabase();
        }

        public async Task<string> GetCache(string key)
            => await _database.StringGetAsync(key);

        public async Task SetCache(string key, string value)
            => await _database.StringSetAsync(key, value);

    }
}
