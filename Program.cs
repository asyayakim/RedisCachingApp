using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace RedisCachingApp;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "master";
        });
        var provider = services.BuildServiceProvider();
        var cache = provider.GetService<IDistributedCache>();

        if (cache == null)
        {
            Console.WriteLine(
                "Error: Could not initialize distributed cache. Ensure Redis is running and configuration is correct.");
            return;
        }

        string cacheKey = "ProductList";
        string cachedData = await cache.GetStringAsync(cacheKey);
        if (cachedData != null)
        {
            Console.WriteLine("Cache hit: " + cachedData);
        }
        else
        {
            Console.WriteLine("Cache miss, fetching from db");
            string productData = "banan 1, honey 2, beans 3";
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            await cache.SetStringAsync(cacheKey, productData, options);
            Console.WriteLine("Data stored in cache" + productData);
        }

        string cacheCounterKey = "CounterShared";
        string cachedValue = await cache.GetStringAsync(cacheKey);
        int counter = cachedValue != null ? int.Parse(cachedValue) : 0;
        counter++;
        await cache.SetStringAsync(cacheCounterKey, counter.ToString());
        Console.WriteLine("Counter: " + counter);
    }

}