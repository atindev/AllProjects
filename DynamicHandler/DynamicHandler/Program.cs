using Newtonsoft.Json;
using System.IO;

namespace DynamicHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader(@"C:\Users\singlaa02\Documents\Visual Studio 2019\Projects\DynamicHandler\DynamicHandler\json1.json"))
            {
                string json = r.ReadToEnd();
                dynamic items = JsonConvert.DeserializeObject(json);
            }
        }
    }
}
