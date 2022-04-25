namespace XamTrial.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            //Httpss.Initialize();
            LoadApplication(new XamTrial.App());
        }
    }
}
