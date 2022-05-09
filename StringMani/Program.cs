using System;
using System.Collections.Generic;
using System.Linq;

namespace StringMani
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(StringCheck("aaabbc", 2));//Solve("aaabbc", 2) Solve("bbbbxyyzzz", 3)
            Console.WriteLine(StringCheck("bbbbxyyzzz", 3));//Solve("aaabbc", 2) Solve("bbbbxyyzzz", 3)

            Console.WriteLine(StringChecknoLinq("aaabbc", 2));//Solve("aaabbc", 2) Solve("bbbbxyyzzz", 3)
            Console.WriteLine(StringChecknoLinq("bbbbxyyzzz", 3));//Solve("aaabbc", 2) Solve("bbbbxyyzzz", 3)


            ////            solve([10, 7, 5, 20, 21, 23, 9])
            ////> [5, 20, 21, 23]

            ////Example #2

            ////solve([-10, -7, -5, -20, 21, 20, 9])
            ////> [-10, -7, -5]

            Console.ReadLine();
        }

        public static int[] Solve(int[] intArray)
        {
            int[] first = new int[intArray.Length];
            int[] second = new int[intArray.Length];

            int previous = int.MinValue;
            int length = 0;
            foreach (var item in intArray)
            {
                if (length > 0 && item > previous)
                {
                    ++length
                    first[] == item;
                }
            }
            return;
        }

        private static string StringCheck(string obj, int k)
        {
            string retString = string.Empty;
            var data = obj.ToCharArray().GroupBy(x => x).Select(x => new St() { charCount = x.Count(), charValue = x.Key }).OrderByDescending(x => x.charCount);
            retString = data.Skip(k - 1).First().charValue.ToString();
            return retString;
        }

        private static string StringChecknoLinq(string obj, int k)
        {
            string retString = string.Empty;
            var data = obj.ToCharArray();
            Dictionary<string, int> lst = new Dictionary<string, int>();
            foreach (var item in data)
            {
                if (lst.Keys.Contains(item.ToString()))
                {
                    lst[item.ToString()] = lst[item.ToString()] + 1;
                }
                else
                {
                    lst.Add(item.ToString(), 1);
                }
            }

            List<KeyValuePair<string, int>> obj1 = lst.ToList();
            obj1.Sort(Compare2);

            obj1.ForEach(x => Console.WriteLine(x.Key + "; " + x.Value));

            retString = obj1[k - 1].Key;
            return retString;
        }

        static int Compare2(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return b.Value.CompareTo(a.Value);
        }

        private class St
        {
            public char charValue { get; set; }
            public int charCount { get; set; }
        }
    }
}
