using Xamarin.Forms;
using XamarinFormsClock.Views;

namespace XamarinFormsClock
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new ClockPage();
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
