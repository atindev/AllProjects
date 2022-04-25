using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public static class KataTitleCaseString
{
    public static string TitleCase(string title, string minorWords = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            Console.WriteLine("1: '" + title + "'");
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            string s = "";
            string[] s23 = title.ToLower().Split(' ');
            if (!string.IsNullOrEmpty(minorWords))
            {
                Console.WriteLine("2: '" + minorWords + "'");
                string[] s1 = minorWords.ToLower().Split(' ');
                foreach (var item in s23)
                {
                    if (s1.Contains(item))
                    {
                        s += item.ToLower() + " ";
                    }
                    else
                    {
                        s += myTI.ToTitleCase(item) + " ";
                    }

                }
            }
            else
            {
                s = myTI.ToTitleCase(title);
            }
            s = s.Trim();
            Console.WriteLine(s);
            return char.ToUpper(s[0]).ToString() + s.Substring(1);
            //return s;
        }
        else
        {
            return "";
        }
    }

    public static int[] ArrayDiff(int[] a, int[] b)
    {
        // Your brilliant solution goes here
        // It's possible to pass random tests in about a second ;)

        ////return a.Except(b).ToArray();
        return a.Where(x => !b.Contains(x)).ToArray();
    }
}
