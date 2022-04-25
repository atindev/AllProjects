using System;
using System.Collections.Generic;
using System.Linq;

public static class KataDeleteNth
{
    public static int[] DeleteNth(int[] arr, int x)
    {
        // var data = arr.Where(x1 => arr.Count(y => y == x1) > x);
        // var data1 = data.Distinct().ToList().SelectMany(x1 => Enumerable.Repeat(x1, x));
        // return arr.Where(x1 => !data.Contains(x1)).Select(x2 => x2).Concat(data1).ToArray();

        //var data = arr.Select((x1, pos) => new { x1, pos, cnts = arr.Count(x2 => x2 == x1).Select((x2, pos) => new { x2, pos }) })

        var data = Enumerable.Empty<int>();
        foreach (var item in arr)
        {
            int cnt = data?.Count(x2 => x2 == item) ?? 0;
            System.Console.WriteLine(item + ": " + cnt);
            if (cnt < x)
            {
                data = data.Append(item);
            }
        }
        return data.ToArray();
    }
}