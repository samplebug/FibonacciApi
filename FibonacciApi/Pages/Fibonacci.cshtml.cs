using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FibonacciApi;

public class FibonacciModel(FibonacciService fibonacciService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? InputValue { get; set; }

    public BigInteger? Result { get; private set; }
    public string? ErrorMessage { get; private set; }

    public void OnGet()
    {
        if (InputValue.HasValue)
        {
            try
            {
                Result = fibonacciService.CalculateFibonacci(InputValue.Value);
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}