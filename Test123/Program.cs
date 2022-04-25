using System;

namespace Test123
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            P2 s = new P2();
            s.asd_default();
            s.asd_abstract();

            P1 s1 = new P1(1);
            s1.asd_default();
            s1.asd_abstract();

            P1 s2 = new P2();
            s2.asd_default();
            s2.asd_abstract();

            P3 s3 = new P1(1);
            s3.asd_abstract();
            s3.asd_virtual();

            P3 s4 = new P2();
            s4.asd_abstract();
            s4.asd_virtual();

            P3.asd_StaticFunction();

            Console.ReadLine();
        }
    }
}
