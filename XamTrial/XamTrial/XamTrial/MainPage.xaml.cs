using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Xamarin.Forms;

namespace XamTrial
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        //bool IsRunning = true;

        private readonly TimeSpan timespan;
        private readonly Action callback;

        private CancellationTokenSource cancellation;

        public MainPage()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            InitializeComponent();
            //Trs.Text = "TRYIAL PAASSED";
            //BitPrice();

            timespan = TimeSpan.FromSeconds(1);
            callback = new Action(() => this.BitPrice());
            cancellation = new CancellationTokenSource();
        }


        static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //var certPublicString = certificate?.GetPublicKeyString();
            //var keysMatch = PUBLIC_KEY == certPublicString;
            return true;
        }


        public async void BitPrice()
        {
            Trs.Text = string.Empty;
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ////ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //var filter = new HttpBaseProtocolFilter();

                //filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
                //filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
                //filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);

                //WebRequestHandler handler = new WebRequestHandler();
                //X509Certificate2 certificate = GetMyX509Certificate();
                //handler.ClientCertificates.Add(certificate);

                //var handler = new HttpClientHandler
                //{
                //    UseProxy = true,
                //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                //};

                using (var httpClient = new HttpClient())
                {
                    #region commented
                    //var response = await httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                    //dynamic t = JsonConvert.DeserializeObject(response);
                    //JObject j = JObject.Parse(response);
                    //var j12 = j["bpi"];
                    //var j13 = j12["USD"];
                    //foreach (var j1 in j13.Children())
                    //{
                    //    Trs.Text += j1.ToString() + " ";
                    //}
                    #endregion

                    #region Commented
                    //var response = await httpClient.GetStringAsync("https://api.exchangeratesapi.io/latest");
                    //var j = (JObject.Parse(response))["rates"];
                    //foreach (JProperty j12 in j.Children())
                    //{
                    //    Trs.Text += j12.Name.ToString() + " : " + j12.Value.ToString() + "\r\n";
                    //}
                    //Trs.Text += response.ToString() + "\r\n";
                    #endregion

                    #region Commneted
                    //httpClient.BaseAddress = new Uri("https://localhost:44329/");

                    ////httpClient.DefaultRequestHeaders.Accept.Clear();
                    ////httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    ////var task = httpClient.PostAsXmlAsync<DeviceRequest>("api/SaveData", request);

                    //var response = await httpClient.GetStringAsync(@"api/values/GetScale/");
                    ////var j = (JObject.Parse(response));
                    ////foreach (JProperty j12 in j.Children())
                    ////{
                    ////    Trs.Text += j12.Name.ToString() + " : " + j12.Value.ToString() + "\r\n";
                    ////}
                    #endregion

                    Scale obj = new Scale("127.0.0.1", 1337);
                    Trs.Text = await obj.GetWeight();

                    Trs.Text += "\r\n" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fffffff");
                }
            }
            catch (Exception ex)
            {
                Trs.Text = "Error" + ex.Message;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            BitPrice();
            DisplayAlert("Added", "Your Data has been Refreshed", "OK");
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            CancellationTokenSource cts = this.cancellation;
            Device.StartTimer(this.timespan, () =>
            {
                if (cts.IsCancellationRequested)
                    return false;
                //Device.BeginInvokeOnMainThread(() => BitPrice());
                this.callback.Invoke();
                return true;
            });
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            #region commented
            //IsRunning = false;
            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    if (IsRunning)
            //    {
            //        Device.BeginInvokeOnMainThread(() => BitPrice());
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //});
            #endregion
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}
