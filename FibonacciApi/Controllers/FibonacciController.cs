using Microsoft.AspNetCore.Mvc;
using FibonacciApi;

[ApiController]
[Route("api/[controller]")]
public class FibonacciController(IFibonacciService fibonacciService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFibonacci([FromQuery] int n)
    {
        if (n < 0)
            return BadRequest("Input must not be negative");

        var result = await fibonacciService.CalculateFibonacciAsync(n);

        return Ok(result.ToString());
    }
}