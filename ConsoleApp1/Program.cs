using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Fn();
            Console.ReadLine();
        }

        public static async void Fn()
        {
            NetworkScaleSettings obj = new NetworkScaleSettings("10.69.40.73", 4305);
            Console.WriteLine(await obj.GetWeight());
        }
    }
}
