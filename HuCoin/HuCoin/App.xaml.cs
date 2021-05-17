using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            OpenStartupPage();
        }

        private void OpenStartupPage()
        {
            if (this.Properties.ContainsKey(AppStatic.IsFirstTimeRunApplicationKey))
            {
                MainPage = new NavigationPage(new Views.MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Views.Walkthorugh.WalkthorughPage());
                this.Properties.Add(AppStatic.IsFirstTimeRunApplicationKey, true);
                this.SavePropertiesAsync();
            }
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