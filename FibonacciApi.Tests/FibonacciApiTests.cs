using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FibonacciApi.Tests;

public class FibonacciApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    readonly WebApplicationFactory<Program> _factory = new();

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(2, "1")]
    [InlineData(5, "5")]
    [InlineData(10, "55")]
    [InlineData(15, "610")]
    public async Task GetFibonacci_ValidInputs_ReturnsCorrectResult(int n, string expected)
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync($"/api/fibonacci?n={n}");
        var result = await response.Content.ReadAsStringAsync();

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFibonacci_BadInput_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("api/Fibonacci?n=-1");

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}