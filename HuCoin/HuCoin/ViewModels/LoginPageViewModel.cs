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
            Xamarin.Essentials.SecureStorage.GetAsync(AppStatic.LastUserLoginKey).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    RequestLogin.Username = task.Result;
                    OnPropertyChanged(nameof(RequestLogin));
                }
            });
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
                using var loadingview = new Components.LoadingView();
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/login", RequestLogin);
                if (response.IsSuccessStatusCode)
                {
                    AppStatic.Token = await response.Content.ReadAsStringAsync();
                    if (RequestLogin.IsRememberMe)
                        await Xamarin.Essentials.SecureStorage.SetAsync(AppStatic.LastUserLoginKey, RequestLogin.Username);

                    if (await Xamarin.Essentials.SecureStorage.GetAsync(AppStatic.HuCoinPinCodeKey) == null)
                        OpenPageAsMainPage(new Views.CreatePinCodePage());
                    else
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
