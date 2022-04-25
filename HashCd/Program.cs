using Newtonsoft.Json.Linq;
using System.IO;

namespace HashCd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string fileName = @"C:\Users\singlaa02\Downloads\a.txt";
            var myJsonString = File.ReadAllText(fileName);

            dynamic myJObject = JObject.Parse(myJsonString);



        }
    }
}
