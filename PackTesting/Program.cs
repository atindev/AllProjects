using Newtonsoft.Json;
using PackTesting.Model;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PackTesting
{
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            ////Console.WriteLine("Hello World!");



        }

        public async Task StartPackTest()
        {
            try
            {
                ApiResult<PackOrderDetails> packOrderDetails = await GetPackOrder();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task<ApiResult<PackOrderDetails>> GetPackOrder()
        {
            ApiResult<PackOrderDetails> packOrderDetails = null;
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return packOrderDetails;
        }

        public async Task<T> HttpCall<T>(HttpMethod type, string url, object data = null)
        {
            T dataResult = default;
            try
            {
                var httpClient = new HttpClient();
                var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                string dataToSend = data != null ? JsonConvert.SerializeObject(data) : string.Empty;
                var dataContent = new StringContent(JsonConvert.SerializeObject(dataToSend), Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                string data1 = null;
                switch (type)
                {
                    case Post:
                        response = await httpClient.PostAsync(url, dataContent);
                        data1 = await response.Content.ReadAsStringAsync();
                        dataResult = JsonConvert.DeserializeObject<T>(data1, settings);
                        break;

                    case HttpPut:
                        response = await httpClient.PutAsync(url, dataContent);
                        data1 = await response.Content.ReadAsStringAsync();
                        dataResult = JsonConvert.DeserializeObject<T>(data1, settings);
                        break;

                    case HttpDelete:
                        response = await httpClient.DeleteAsync(url);
                        data1 = await response.Content.ReadAsStringAsync();
                        dataResult = JsonConvert.DeserializeObject<T>(data1, settings);
                        break;

                    case HttpGet:
                        response = await httpClient.GetAsync(url);
                        data1 = await response.Content.ReadAsStringAsync();
                        dataResult = JsonConvert.DeserializeObject<T>(data1, settings);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dataResult = default;
            }
            return dataResult;
        }
    }
}
