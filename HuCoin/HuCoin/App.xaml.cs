using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using localizer = HuCoin.Utils.LocalizationResourceManager;

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
            if (this.Properties.TryGetValue(AppStatic.LanguageKey, out object culture))
                localizer.Instance.SetCulture(new System.Globalization.CultureInfo(culture as string));

            if (this.Properties.ContainsKey(AppStatic.IsFirstTimeRunApplicationKey))
            {
                var page = new Views.LoginPage();
                page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(path: nameof(localizer.FlowDirection), source: localizer.Instance));

                MainPage = new NavigationPage(page);
            }
            else
            {
                var page= new Views.Walkthorugh.WalkthorughPage(); 
                page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(path: nameof(localizer.FlowDirection), source: localizer.Instance));

                MainPage = page;
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