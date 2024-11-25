using System.Numerics;

namespace FibonacciApi;

public class FibonacciService
{
    public BigInteger CalculateFibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("Input must not be negative");

        if (n == 0)
            return 0;
        if (n == 1)
            return 1;

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