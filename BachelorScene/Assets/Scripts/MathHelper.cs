using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MathHelper
{
    public static List<int> GetDigitsForNumber(int n, int padding = 0)
    {
        List<int> digits = new List<int>();

        while (n > 0)
        {
            digits.Add(n % 10);
            n /= 10;
        }

        while (digits.Count < padding)
        {
            digits.Add(0);
        }

        digits.Reverse();

        return digits;
    }

    public static float Approach(float val, float target, float maxMove)
    {
        if (val <= target)
        {
            return Math.Min(val + maxMove, target);
        }

        return Math.Max(val - maxMove, target);
    }

    public static float Mod(float n, float m)
    {
        return ((n % m) + m) % m;
    }

    public static int Mod(int n, int m)
    {
        return ((n % m) + m) % m;
    }
}
