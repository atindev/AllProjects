using Simple.OData.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Try
{
    class Program
    {
        static HttpClient client1 = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Try2().GetAwaiter().GetResult();
            TrySimpleOdata().GetAwaiter().GetResult();
            //Task.Run(async () =>
            //{
            //    var responseMessage = await client1.GetAsync("http://mfgsystemsprototypeapi.azurewebsites.net/odata/$metadata");
            //    var ss = await responseMessage.Content.ReadAsStringAsync();
            //    Console.ReadLine();
            //}).GetAwaiter().GetResult();
        }

        public static async Task Try2()
        {
            try
            {
                HttpClient client1 = new HttpClient();
                HttpResponseMessage clientresponse = await client1.GetAsync("http://mfgsystemsprototypeapi.azurewebsites.net/odata/$metadata");//$metadata
                string s = await clientresponse.Content.ReadAsStringAsync();
                Console.WriteLine(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task TrySimpleOdata()
        {
            try
            {
                string baseUrl = @"https://mfgsystemsprototypeapi.azurewebsites.net/odata/";
                string buildUrl = @"MfgOrder?$apply=filter((OrderType%20eq%20%27ZCOM%27%20or%20OrderType%20eq%20%27ZFRM%27)%20and%20(FormulaCode%20ne%20%27TEST%27))/aggregate(Quantity%20with%20sum%20as%20Quantity)";

                var settings = new ODataClientSettings(GetHttpClient(baseUrl));
                var client = new ODataClient(settings);
                var reponse = await client.FindEntriesAsync(buildUrl);

                Console.WriteLine(reponse.ToString());
                var res = await client.For("MfgOrder").Top(2).FindEntriesAsync();

                Console.WriteLine(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static HttpClient httpClient;

        public static HttpClient GetHttpClient(string baseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            return httpClient;
        }
    }
}
