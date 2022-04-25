using Xamarin.Forms;

namespace TryCharts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new chartPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
