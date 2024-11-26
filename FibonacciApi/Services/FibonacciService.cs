using System.Numerics;
using System.Text.Json;
using FibonacciApi.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Prometheus;

namespace FibonacciApi;

public class FibonacciService(IDistributedCache cache, ILogger<FibonacciService> logger) : IFibonacciService
{
    const double CacheSpanMinutes = 10;

    // Custom Prometheus Counter for Fibonacci calculations
    static readonly Counter FibonacciCounter = Metrics.CreateCounter(
        "fibonacci_calculations_total",
        "Total number of Fibonacci calculations performed.");

    // async wrapper for distributed cache and scaling in general
    public async Task<BigInteger> CalculateFibonacciAsync(int n)
    {
        if (n < 0)
        {
            logger.LogError("Invalid input. Must be non-negative");
            throw new ArgumentException("Input must be non-negative");
        }

        FibonacciCounter.Inc(); // Increment the custom metric

        logger.LogInformation($"Calculating Fibonacci for {n}");
        var options = new JsonSerializerOptions();
        options.Converters.Add(new BigIntegerConverter());

        var cachedValue = await cache.GetStringAsync(n.ToString());
        if (cachedValue != null)
        {
            try
            {
                var deserializedValue = JsonSerializer.Deserialize<BigInteger>(cachedValue, options);
                logger.LogInformation($"Fetched cached result of {cachedValue}");
                logger.LogInformation($"Fetched cached result of {deserializedValue}");
                return deserializedValue;
            }
            catch (JsonException)
            {
                logger.LogError($"Invalid cached data for key {n}");
                Console.WriteLine($"Invalid cached data for key {n}");
            }
        }


        var result = CalculateFibonacci(n);
        logger.LogInformation($"Result {result.ToString()}");

        // Cache the result
        logger.LogDebug($"Caching Fibonacci for {n}");
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheSpanMinutes)
        };

        var serializedResult = JsonSerializer.Serialize(result, options);

        logger.LogInformation("Storing Fibonacci result {Result} for input {Input} in cache", result, n);
        await cache.SetStringAsync(n.ToString(), serializedResult, cacheOptions);
        logger.LogInformation("Cached result stored successfully.");


        logger.LogDebug($"Finished calculations for {n}");

        return result;
    }

    // linear approach
    BigInteger CalculateFibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("Input cannot be negative");

        if (n <= 1)
            return n;

        BigInteger a = 0,
                   b = 1,
                   result = 0;

        for (int i = 2; i <= n; i++)
        {
            result = a + b;
            a = b;
            b = result;
        }

        return result;
    }
}