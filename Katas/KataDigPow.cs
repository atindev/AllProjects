using System;
using System.Linq;
public static class KataDigPow
{
    public static long digPow(int n, int p)
    {
        // your code
        int a1 = -1;
        double sum = 0;
        var d = n.ToString().ToCharArray().ToList();
        d.ForEach(x => System.Console.WriteLine(x));
        for (int i = 0; i < d.Count; i++)
        {
            char item = d[i];
            double x = Convert.ToDouble(item.ToString());
            double y = p + i;
            System.Console.WriteLine(x + ":" + y);
            sum += System.Math.Pow(x, y);
        }
        if (sum % n == 0)
        {
            a1 = (int)(sum / n);
        }
        return a1;
    }
}