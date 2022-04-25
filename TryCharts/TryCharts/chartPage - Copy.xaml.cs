using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TryCharts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class chartPageCopy : ContentPage
    {
        private readonly ViewModel viewmodel;
        public chartPageCopy()
        {
            viewmodel = new ViewModel();
            InitializeComponent();
            ////viewmodel.FormChart(barChart);
            BindingContext = viewmodel;
        }
    }
}