using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace HuCoin.ViewModels
{
    public class ForgetPageViewModel : BaseViewModel
    {
        public ICommand SendEmailCommand { get; set; }
        public ForgetPageViewModel()
        {
            SendEmailCommand = new Command(SendEmail);
        }
        private void SendEmail()
        {
            using var loadingview = new Components.LoadingView();
            Xamarin.Essentials.Browser.OpenAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/forgotpassword");
        }
    }
}
