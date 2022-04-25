using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TryCharts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class chartPage : ContentPage
    {
        private readonly ViewModel viewmodel;

        private static Timer wdTimer = new Timer(1000);
        ////private static Timer wdTimer1 = new Timer(1000);

        public chartPage()
        {
            viewmodel = new ViewModel();
            InitializeComponent();
            ////viewmodel.FormChart(barChart);

            wdTimer.Elapsed += Check;
            wdTimer.Start();
            wdTimer.Enabled = true;

            ////wdTimer1.Elapsed += CheckTare;
            ////wdTimer1.Start();
            ////wdTimer1.Enabled = true;

            ////BindingContext = viewmodel;
        }

        private void Check(object sender, ElapsedEventArgs e)
        {
            Set();
        }

        private void CheckTare(object sender, ElapsedEventArgs e)
        {
            SetTare();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            ////SetTare();
            Set();
        }

        private void Set()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var te = await viewmodel.GetWeight();
                ////var te = await viewmodel.TareWeight();
                WeightSuccess.Text = $"{te?.success == true}";
                if (te?.success == true)
                {
                    Weight.Text = te.ScaleNetText;
                }
                else
                {
                    var stg1 = string.IsNullOrEmpty(te?.ErrorText) ? "NA" : te.ErrorText;
                    WeightError.Text = stg1;
                    System.Diagnostics.Debug.WriteLine("chartPage " + Tare.Text);
                }

                ////te = await viewmodel.GetTare();
                ////////var te = await viewmodel.TareWeight();
                ////stg1 = string.IsNullOrEmpty(te.ErrorText) ? "NA" : te.ErrorText;
                ////stg = te.success ? te.ScaleNetText : stg1;
                ////Tare.Text = $"{te.success}: {stg}";
                ////if (te?.success != true)
                ////{
                ////    System.Diagnostics.Debug.WriteLine("chartPage " + Tare.Text);
                ////}
            });
        }

        private void SetTare()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var te = await viewmodel.GetTare();
                ////var te = await viewmodel.TareWeight();
                TareSuccess.Text = $"{te?.success == true}";
                if (te?.success == true)
                {
                    Tare.Text = te.ScaleNetText;
                }
                else
                {
                    var stg1 = string.IsNullOrEmpty(te?.ErrorText) ? "NA" : te.ErrorText;
                    TareError.Text = stg1;
                    System.Diagnostics.Debug.WriteLine("chartPage " + Tare.Text);
                }
            });
        }
    }
}