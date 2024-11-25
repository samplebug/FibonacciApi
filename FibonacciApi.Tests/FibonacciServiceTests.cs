using System.Numerics;

namespace FibonacciApi.Tests;

public class FibonacciServiceTests
{
    readonly FibonacciService _fibonacciService = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(5, 5)]
    [InlineData(10, 55)]
    [InlineData(15, 610)]
    public void CalculateFibonacci_ValidInputs_ReturnsCorrectResults(int input, long expected)
    {
        // act
        var result = _fibonacciService.CalculateFibonacci(input);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateFibonacci_InvalidInput_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => _fibonacciService.CalculateFibonacci(-1));
    }

    [Fact]
    public void CalculateFibonacci_BigInput_ReturnsBigInteger()
    {
        var result = _fibonacciService.CalculateFibonacci(555);

        Assert.IsType<BigInteger>(result);
    }
}