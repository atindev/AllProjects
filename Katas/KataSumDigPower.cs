using System;
using System.Collections.Generic;
using System.Linq;

public static class KataSumDigPower
{
    public static long[] SumDigPow(long a, long b)
    {
        // your code
        IEnumerable<long> dataRange = Enumerable.Empty<long>();
        for (long i = a; i <= b; i++)
        {
            if (i.IsEureka())
                dataRange = dataRange.Append(i);
        }
        return dataRange.ToArray();
    }

    private static bool IsEureka(this long a)
    {
        long sum = 0;
        a.ToString().ToCharArray().Select((x, pos) => new { num = char.GetNumericValue(x), position = pos + 1 }).ToList().ForEach(x => sum += (long)(Math.Pow(x.num, x.position)));
        return sum == a;
    }
}