using System;
using System.Linq;


////Replace each element in an integer array with the multiplication of all other elements of array.
////e.g.
////Input = {2, 1, 4, 9 , 1}
////Output = { 36, 72, 18, 8}

////1.Identify test cases.
////2. Write a program/function for the logic.

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var data = new int[] { 2, 1, 4, 9, 2 };
            //Check(data);
            data = new int[] { 2, 1, 4, 9, 100, 0 };
            var res = Getint(data);

            data = new int[] { 2, 1, 0, 9, 100, 0 };
            res.ToList().ForEach(x => Console.WriteLine(x + ", "));
            //data = null;
            //Check(data);
            //data = new int[] { };
            //Check(data);
            //data = new int[] { -2, 1, 4, 9, 2 };
            //Check(data);
            //data = new int[] { -2, -1, 4, 9, 2 };
            //Check(data);
            Console.ReadLine();
        }

        private static void Check(int[] data)
        {
            var data1 = CalculateMultiples(data);
            Console.WriteLine();
            data?.ToList().ForEach(x => Console.Write(x + ", "));
            Console.WriteLine();
            data1?.ToList().ForEach(x => Console.Write(x + ", "));
        }

        private static int[] CalculateMultiples(int[] dataInput)
        {
            int[] result;
            long multiple = GetMultiple(dataInput);
            result = dataInput?.Select(x => x != 0 ? (int)multiple / x : 0).ToArray() ?? new int[0] { };
            return result;
        }

        private static long GetMultiple(int[] dataInput)
        {
            long multiple = 1;
            dataInput?.ToList().ForEach(x => multiple = x * multiple);
            return multiple;
        }

        private static int[] Getint(int[] dataInput)
        {
            int[] dataInputResult = dataInput;
            for (int i = 0; i < dataInput.Length; i++)
            {
                var listInt = dataInput.ToList();
                listInt.RemoveAt(i);
                if (!listInt.Contains(0))
                {
                    Console.WriteLine(0);
                }
                dataInputResult[i] = (int)GetMultiple(listInt.ToArray());
            }
            return dataInputResult;
        }
    }
}
