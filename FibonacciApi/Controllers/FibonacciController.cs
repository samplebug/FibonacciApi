using Microsoft.AspNetCore.Mvc;
using FibonacciApi;

[ApiController]
[Route("api/[controller]")]
public class FibonacciController(FibonacciService fibonacciService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetFibonacci([FromQuery] int n)
    {
        if (n < 0)
            return BadRequest("A number must not be negative");

        var result = fibonacciService.CalculateFibonacci(n);

        return Ok(result.ToString());
    }
}