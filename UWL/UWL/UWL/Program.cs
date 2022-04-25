using Opc.UaFx.Client;
using System;

namespace UWL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                //var client = new OpcClient("opc.tcp://localhost:57888/OpcExpert");
                var client = new OpcClient("opc.tcp://10.100.1.83:49320");
                client.Connect();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
