using System.Numerics;

namespace FibonacciApi;

public interface IFibonacciService
{
    Task<BigInteger> CalculateFibonacciAsync(int n);
}