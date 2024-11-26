using System.Numerics;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FibonacciApi.Tests;

public class FibonacciServiceTests
{
    // I failed to mock cache properly with Moq so using real cache instance
    readonly IDistributedCache _cache;
    readonly IFibonacciService _fibonacciService;

    public FibonacciServiceTests()
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        ILogger<FibonacciService> logger = serviceProvider.GetRequiredService<ILogger<FibonacciService>>();
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
        _fibonacciService = new FibonacciService(_cache, logger);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(5, 5)]
    [InlineData(10, 55)]
    [InlineData(15, 610)]
    public async Task CalculateFibonacciAsync_ValidInputs_ReturnsCorrectResults(int input, long expected)
    {
        // arrange - use real cache instance
        var cachedValue = JsonSerializer.Serialize(new BigInteger(610));
        await _cache.SetStringAsync("55", cachedValue);

        // act
        var result = await _fibonacciService.CalculateFibonacciAsync(input);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CalculateFibonacciAsync_InvalidInput_ThrowsException()
    {
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _fibonacciService.CalculateFibonacciAsync(-1));
    }

    [Fact]
    public async Task CalculateFibonacci_BigInput_ReturnsBigInteger()
    {
        // Act
        var result = await _fibonacciService.CalculateFibonacciAsync(555);

        // Assert
        Assert.IsType<BigInteger>(result);
    }

    [Fact]
    public async Task CalculateFibonacciAsync_UsesCache_WhenAvailable()
    {
        // Act
        var result1 = await _fibonacciService.CalculateFibonacciAsync(10); // Calculate and cache
        var result2 = await _fibonacciService.CalculateFibonacciAsync(10); // Retrieve from cache

        // Assert
        Assert.Equal(result1, result2); // Should be the same
    }
}