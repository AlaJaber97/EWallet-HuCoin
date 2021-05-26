using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class ForgotPasswordPageViewModel : BaseViewModel
    {
        public ICommand OpenBrowserPageCommand { get; set; }
        public ForgotPasswordPageViewModel()
        {
            OpenBrowserPageCommand = new Command(OpenBrowserPage);
        }
        private void OpenBrowserPage()
        {
            Xamarin.Essentials.Browser.OpenAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/forgotpassword");
        }
    }
}
