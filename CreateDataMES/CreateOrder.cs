using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataMES
{
    public class CreateOrder
    {
        public async Task CreateOrders(string token, int newStartOrderNum, int createOrderNum = 50, string fileName = @"Order.json", string envUrl = @"usendevmes")
        {
            try
            {
                HttpClient httpClientObject = new HttpClient();

                httpClientObject.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjcxZjFkN2YyLWIzMjMtNDEzYy1iOTVmLWY1NDJmNDViZjJjNCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJTSU5HTEFBMDIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJBVElOLlNJTkdMQSIsImV4cCI6MTYxNDE2NjM2MywiaXNzIjoiTUVTIiwiYXVkIjoiTUVTIn0.JMyUHyR3Q2E6bCqoDIN3EjL9VZFHXfG4LR1ctsLwbqk");

                ////var myJsonString = File.ReadAllText(@"Order.json");
                var myJsonString = File.ReadAllText(fileName);
                dynamic myJObject = JObject.Parse(myJsonString);

                List<Task<HttpResponseMessage>> orders = new List<Task<HttpResponseMessage>>();

                for (int i = 0; i < createOrderNum; i++, newStartOrderNum++)
                {
                    ////string PROCESSORDER = "300000000" + order.ToString("D3");
                    ////string BATCHNUMBER = "300" + order.ToString("D3");

                    string PROCESSORDER = "800000000" + newStartOrderNum.ToString("D11");
                    string BATCHNUMBER = "DTC" + newStartOrderNum.ToString("D6");

                    myJObject.PROCESSORDER = PROCESSORDER;
                    myJObject.BATCHNUMBER = BATCHNUMBER;

                    var json = JsonConvert.SerializeObject(myJObject);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    orders.Add(httpClientObject.PostAsync($"https://{envUrl}.westpharma.net:30001/inboundinterfaceapi/api/MfgInterface/BRDI/Order", data));

                    ////var postresult = await httpClient.PostAsync(@"https://usendvvmes.westpharma.net:30001/inboundinterfaceapi/api/MfgInterface/BRDI/Order", data);
                    ////var postresult = await httpClient.PostAsync(@"https://usendvvmes.westpharma.net:30001/inboundinterfacehandlerapi/api/MfgOrderHandler/", data);
                    ////var postresult = await httpClient.PostAsync(@"https://westmfginterfacedev.azurewebsites.net/api/MfgInterface/BRDI/Order", data);
                }
                await Task.WhenAll(orders);

                orders.ForEach(x => Console.WriteLine(x.Result.StatusCode.ToString() + ": " + (x.Result.StatusCode != System.Net.HttpStatusCode.OK ? x.Result.Content.ReadAsStringAsync().Result : "Ok")));
                ////Console.WriteLine(postresult.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
