using System;
using System.Data;

namespace Odcb
{
    class Program
    {
        protected Program()
        { }

        static void Main(string[] args)
        {
            OldOracle obj1 = new OldOracle();
            DataTable dt = obj1.GetData();

            NewOracle obj = new NewOracle();
            DataTable dt1 = obj.GetData();

            if (dt1 == dt)
            {
                Console.WriteLine("HAHAH");
            }
        }
    }
}