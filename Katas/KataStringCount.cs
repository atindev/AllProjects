using System;
using System.Collections.Generic;
using System.Linq;

public static class KataStringCount
{
    public static Dictionary<char, int> Count(string str)
    {
        Dictionary<char, int> result = new Dictionary<char, int>();
        if (!string.IsNullOrEmpty(str))
            result = str.ToCharArray().GroupBy(x => x).Select(x => new KeyValuePair<char, int>(x.Key, x.Count())).ToDictionary(x => x.Key, x => x.Value);
        return result;
    }
}