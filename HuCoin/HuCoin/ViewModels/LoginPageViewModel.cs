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
    public class LoginPageViewModel : BaseViewModel
    {
        public ICommand OpenRegisterPageCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public BLL.Models.RequestLogin RequestLogin { get; set; }
        public LoginPageViewModel()
        {
            RequestLogin = new BLL.Models.RequestLogin();
            OpenRegisterPageCommand = new Command(OpenRegisterPage);
            SignInCommand = new Command(()=> SingIn().ConfigureAwait(false));
        }
        private void OpenRegisterPage()
        {
            OpenPage(new Views.RegisterPage());
        }
        private async Task SingIn()
        {
            if (!string.IsNullOrWhiteSpace(RequestLogin.Username) &&
                !string.IsNullOrWhiteSpace(RequestLogin.Password))
            {
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://192.168.0.199:5000/api/account/login", RequestLogin);
                if (response.IsSuccessStatusCode)
                {
                    AppStatic.Token = await response.Content.ReadAsStringAsync();
                    OpenPageAsMainPage(new Views.MainPage());
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                }
            }
            else
            {
                await DisplayAlert("Required Field", "Please, don't leave empty fields", "Ok").ConfigureAwait(false);
            }
        }
    }
}
