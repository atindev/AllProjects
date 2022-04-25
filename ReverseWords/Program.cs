using System;
using System.Collections.Generic;
using System.Text;

namespace ReverseWords
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string c = "Today is my interview @  WatchGuard";//// Console.ReadLine();
            string outString = Reverse(c);
            Console.WriteLine(outString);

            c = "test string /t /r /n &nbsp; here";
            outString = Reverse(c);
            Console.WriteLine(outString);

            Console.ReadLine();
        }

        public static string Reverse(string input)
        {
            List<string> ret = new List<string>();
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder word = new StringBuilder();
                foreach (var item in input)
                {
                    if (item != ' ')
                    {
                        word.Append(item);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word.ToString()))
                        {
                            ret.Insert(0, word.ToString());
                            word.Clear();
                        }
                        ret.Insert(0, " ");
                    }
                }
                if (!string.IsNullOrEmpty(word.ToString()))
                {
                    ret.Insert(0, word.ToString());
                    word.Clear();
                }
            }
            return String.Join("", ret);
        }
    }
}


////Reverse the words in a string.
////Example, Input String = "Today is my interview @  WatchGuard"
////Output String = "WatchGuard  @ interview my is Today".
////** Note: Preserve all blank spaces between words while reversing the string.