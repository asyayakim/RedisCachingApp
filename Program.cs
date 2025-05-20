using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace RedisCachingApp;

class Program
{
    // static async Task Main(string[] args)
    // {
    //     var services = new ServiceCollection();
    //     services.AddStackExchangeRedisCache(options =>
    //     {
    //         options.Configuration = "localhost:6379";
    //         options.InstanceName = "CacheExample";
    //     });
    //     var provider = services.BuildServiceProvider();
    //     var cache = provider.GetService<IDistributedCache>();
    //
    //     if (cache == null)
    //     {
    //         Console.WriteLine(
    //             "Error: Could not initialize distributed cache. Ensure Redis is running and configuration is correct.");
    //         return;
    //     }
    //
    //     string cacheKey = "ProductList";
    //     string cachedData = await cache.GetStringAsync(cacheKey);
    //     if (cachedData != null)
    //     {
    //         Console.WriteLine("Cache hit: " + cachedData);
    //     }
    //     else
    //     {
    //         Console.WriteLine("Cache miss, fetching from db");
    //         string productData = "banan 1, honey 2, beans 3";
    //         var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
    //         await cache.SetStringAsync(cacheKey, productData, options);
    //         Console.WriteLine("Data stored in cache" + productData);
    //     }
    //
    //     string cacheCounterKey = "CounterShared";
    //     string cachedValue = await cache.GetStringAsync(cacheKey);
    //     int counter = cachedValue != null ? int.Parse(cachedValue) : 0;
    //     counter++;
    //     await cache.SetStringAsync(cacheCounterKey, counter.ToString());
    //     Console.WriteLine("Counter: " + counter);
    // }
    
    //Added policies for cache
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "CacheExample";
        });
        var provider = services.BuildServiceProvider();
        var cache = provider.GetService<IDistributedCache>();

        Console.WriteLine("Redis cache setup completed");
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2),
        };
        await cache.SetStringAsync("MyKey", "Sample Key", cacheEntryOptions);
        Console.WriteLine("Key set with absolute and sliding expiration");

        async Task InvalidateCache(IDistributedCache cache, string key)
        {
            await cache.RemoveAsync(key);
            Console.WriteLine($"Key {key} removed from cache");
        }
        Console.WriteLine("Press any key to invalidate the cache");
        Console.ReadKey();
        
        
        await InvalidateCache(cache, "MyKey");
        async Task<string> GetCachedData(IDistributedCache cache, string key)
        {
            var cachedValue = await cache.GetStringAsync(key);


            if (cachedValue != null)
            {
                Console.WriteLine($"Cache hit: {cachedValue}");
                return cachedValue;
            }

            Console.WriteLine("Cache miss: Fetching new data...");
            var newValue = "Fetched Task Data";
            await cache.SetStringAsync(key, newValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });
            Console.WriteLine("New data cached.");
            return newValue;
        }

        for (int i = 0; i < 5; i++)
        {
            await GetCachedData(cache, "taskKey");
            await Task.Delay(TimeSpan.FromSeconds(10)); // Simulate waiting time
        }
    }
}